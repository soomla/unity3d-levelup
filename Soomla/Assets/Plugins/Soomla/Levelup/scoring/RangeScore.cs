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


namespace Soomla.Levelup
{
	public class RangeScore : Score
	{
		public SRange Range;

		public RangeScore(string scoreId, string name, SRange range)
			: base(scoreId, name)
		{
			Range = range;
		}

		public RangeScore(string scoreId, string name, bool higherBetter, SRange range)
			: base(scoreId, name, higherBetter)
		{
			Range = range;
			// descending score should start at the high bound
			if (!HigherBetter) {
				StartValue = range.High;
			}
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RangeScore(JSONObject jsonScore)
			: base(jsonScore)
		{
			Range = new SRange(jsonScore[LUJSONConsts.LU_SCORE_RANGE]);
			// descending score should start at the high bound
			if (!HigherBetter) {
				StartValue = range.High;
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SCORE_RANGE, Range.toJSONObject());

			return obj;
		}

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

		public override void Dec(double amount) {
			
			// Don't dencrement if we've hit the range's lowest value
			if (_tempScore <= Range.Low) {
				return;
			}

			if ((_tempScore-amount) < Range.Low) {
				amount = _tempScore - Range.Low;
			}

			base.dec(amount);
		}

		public override void SetTempScore(double score) {
			if (score > Range.High) {
				score = Range.High;
			}
			if (score < Range.Low) {
				score = Range.Low;
			}

			base.SetTempScore(score);
		}

		// TODO: register for events and handle them

		public class SRange {

			public double Low;
			public double High;

			public SRange(double low, double high) {
				Low = low;
				High = high;
				// TODO: throw exception if low >= high
			}

			public SRange(JSONObject jsonObject) {
				Low = jsonObject[LUJSONConsts.LU_SCORE_RANGE_LOW].n;
				High = jsonObject[LUJSONConsts.LU_SCORE_RANGE_HIGH].n;
			}

			public JSONObject toJSONObject(){
				JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
				jsonObject.Add(LUJSONConsts.LU_SCORE_RANGE_LOW, Low);
				jsonObject.Add(LUJSONConsts.LU_SCORE_RANGE_HIGH, High);
				
				return jsonObject;
			}
		}
	}
}

