/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.using System;

using System;

namespace Soomla.Levelup
{

	/// <summary>
	/// A specific type of <c>Score</c> that has an associated range. The <c>Score</c>'s 
	/// value can be only inside the range of values. For example, a shooting <c>Score</c>
	/// can be on a scale of 10 to 100 according to the user's performance in the game.
	/// </summary>
	public class RangeScore : Score
	{
		public SRange Range;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="range">Range that the <c>Score</c> value must reside in.</param>
		public RangeScore(string id, SRange range)
			: base(id)
		{
			Range = range;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="higherBetter">If set to <c>true</c> then higher is better.</param>
		/// <param name="range">Range that the <c>Score</c> value must reside in.</param>
		public RangeScore(string id, string name, bool higherBetter, SRange range)
			: base(id, name, higherBetter)
		{
			Range = range;
			// descending score should start at the high bound
			if (!HigherBetter) {
				StartValue = range.High;
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonScore">JSON score.</param>
		public RangeScore(JSONObject jsonScore)
			: base(jsonScore)
		{
			Range = new SRange(jsonScore[LUJSONConsts.LU_SCORE_RANGE]);
			// descending score should start at the high bound
			if (!HigherBetter) {
				StartValue = Range.High;
			}
		}
		
		/// <summary>
		/// Converts this <c>Score</c> to JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SCORE_RANGE, Range.toJSONObject());

			return obj;
		}

		/// <summary>
		/// Increases this <c>Score</c> by the given amount after checking that it will stay 
		/// within the range.
		/// </summary>
		/// <param name="amount">Amount to increase by.</param>
		public override void Inc(double amount) {
			
			// Don't increment if we've hit the range's highest value
			if (_tempScore >= Range.High) {
				return;
			}

			if ((_tempScore+amount) > Range.High) {
				amount = Range.High - _tempScore;
			}

			base.Inc(amount);
		}

		/// <summary>
		/// Decreases this <c>Score</c> by the given amount after checking that the <c>Score</c>
		/// will stay within the range.
		/// </summary>
		/// <param name="amount">Amount to decrease by.</param>
		public override void Dec(double amount) {
			
			// Don't dencrement if we've hit the range's lowest value
			if (_tempScore <= Range.Low) {
				return;
			}

			if ((_tempScore-amount) < Range.Low) {
				amount = _tempScore - Range.Low;
			}

			base.Dec(amount);
		}

		/// <summary>
		/// Sets the temp score to be the given <c>score</c>, after making sure that the 
		/// it will stay within the range.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="onlyIfBetter">If <c>Score</c> is better than the given <c>score</c>  
		/// then this value should be <c>true</c>.</param>
		public override void SetTempScore(double score, bool onlyIfBetter) {
			if (score > Range.High) {
				score = Range.High;
			}
			if (score < Range.Low) {
				score = Range.Low;
			}
			
			base.SetTempScore(score, onlyIfBetter);
		}

		/// <summary>
		/// Each <c>RangeScore</c> has a range (<c>SRange</c>).
		/// </summary>
		public class SRange {

			public double Low;
			public double High;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="low">Lowest value of the range.</param>
			/// <param name="high">Highest value of the range.</param>
			public SRange(double low, double high) {
				Low = low;
				High = high;
				// TODO: throw exception if low >= high
			}

			/// <summary>
			/// Contructor.
			/// </summary>
			/// <param name="jsonObject">JSON object.</param>
			public SRange(JSONObject jsonObject) {
				Low = jsonObject[LUJSONConsts.LU_SCORE_RANGE_LOW].n;
				High = jsonObject[LUJSONConsts.LU_SCORE_RANGE_HIGH].n;
			}

			/// <summary>
			/// Converts the range to a JSON object.
			/// </summary>
			/// <returns>The JSON object.</returns>
			public JSONObject toJSONObject(){
				JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
				jsonObject.AddField(LUJSONConsts.LU_SCORE_RANGE_LOW, Convert.ToInt32(Low));
				jsonObject.AddField(LUJSONConsts.LU_SCORE_RANGE_HIGH, Convert.ToInt32(High));
				
				return jsonObject;
			}
		}
	}
}

