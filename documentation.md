
**unity3d-store Documentation Conventions**
=====================


##Order of Code


1. SOOMLA Copyright message (see below)
2. All class members
3. Constructors
4. Public methods
5. Setters and Getters
6. Protected methods
7. Private methods


##SOOMLA's Copyright Message


SOOMLA is licensed under the Apache License. This copyright message must be included in every file at the top.

```
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
/// limitations under the License.
```

##General Guidelines

- A doc comment must precede a class, method declaration, constructor, or field.
- For private methods, documentation comments are optional - if the method is non-trivial and complex, you should document it, otherwise leave it with no comments.
- Do not write documentation for trivial getter and setter methods.
- Do not write documentation for trivial fields.
- All comment lines should start with three forwards slashes (`///`).
- Limit doc-comment lines to 120 characters.

##XML Comments
All XML comments begin with three forward slashes (`///`).

The main tags to use are:

- `<summary>`
Use `<summary></summary>` for descriptions of classes, method, and fields.
```
/// <summary>
/// Does something ...
/// </summary>
```
- `<param>`
Use `<param></param>` for each parameter of a method. Notice that the param tag includes the attribute 'name'.
```
/// <param name='productId'>
/// The Id of the current item in the mobile market.
/// </param>
```
- `<returns>`
Use `<returns></returns>` for the return value of a method.
```
/// <returns>
/// Description of the return value of this method.
/// </returns>
```
- `<exception>`
Use the `<exception></exception>` tags to document any exceptions the method may throw. The exception tag includes the attribute 'cref', which should be the name of the exception.
```
/// <exception cref="SampleException">
/// Normally, a discussion on any exceptions thrown would go here.
/// </exception>
```
- `<see>`
Use `<see>` to specify a hyperlink. The see tag is usually used inline and includes the attribute 'cref'.
```
/// <summary>	 
/// Initializes <see cref="com.soomla.unity.MarketItem"/> class.
/// </summary>
```
- `<c>`
Use the `<c>` to indicate code within text.
```
/// <summary>
/// This class represents an item in the market.
/// <c>MarketItem</c> is only used for <c>PurchaseWithMarket</c> purchase type.
/// </summary>
```
For more types of comments see http://msdn.microsoft.com/en-us/library/5ast78ax.aspx. 

##Description and Examples

**Classes and Interfaces**

 - State the purpose of this class or interface.
 - Include possible ‘Real Game Examples’ to make the purpose clearer.
 - Include any important notes or warnings the user should know.
 - Declare the inheritance path like so: 
`Inheritance: Class > Superclass > ...` 
- Do not declare an inheritance path that consists of only one level.
- Use `<see cref="reference"/>` to make the superclasses available as links (see example below).
Example:
``` 
/// <summary>
/// Single use virtual goods are the most common type of <c>VirtualGood</c>.
/// 
/// The <c>SingleUseVG</c>'s characteristics are:
/// 1. Can be purchased an unlimited number of times.
/// 2. Has a balance that is saved in the database. Its balance goes up when you "give" it or
///   "buy" it. The balance goes down when you "take" or (unfriendly) "refund" it.
/// 
/// Real Game Examples: 'Hat', 'Sword', 'Muffin'
/// 
/// NOTE: In case you want this item to be available for purchase with real money
/// you will need to define the item in the market (App Store, Google Play...).
/// 
/// Inheritance: SingleUseVG >
/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
/// <see cref="com.soomla.store.domain.VirtualItem"/>
/// </summary>
public class SingleUseVG : VirtualGood {
    ...
}
```
<br>
**Methods**

 - The description should clearly state the purpose of the method.
 - Surround the description with `<summary>` tags. Each tag should be on a line of its own, separated from the description.
 - If this is a constructor, write “Constructor.” as the first line, continue the rest of the description on the next line.
 - The description must begin with a 3rd person descriptive verb
     - CORRECT: “Checks…”, “Converts…”, “Retrieves...”, etc.. 
     - INCORRECT: “Check”, “This method does…” 
     - The description of the method cannot begin with “Returns…”
 - Include possible examples to make the purpose of the method clearer.
 - Include any important notes or warnings the user should know.
 - Overriding Methods: 
     - If the overriding method’s description is exactly the same as its parent method’s description, write: “see parent”. Otherwise, write a description.
 - When referring to a method in a comment use the `<c>` tags. 

Example:
```
/// <summary>
/// Retrieves the balance of the virtual item with the given <c>itemId</c>.
/// </summary>
/// <param name="itemId">Id of the virtual item to be fetched.</param>
/// <returns>Balance of the virtual item with the given item id.</returns>
/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
public static int GetItemBalance(string itemId) {
    ...
}
```
<br>
**Class Members**

For non-trivial fields, write a clear description of the variable. Many fields have names that completely explain their purpose - for these there is no need to write comments. 

- If the description fits on the same line as the code, insert it following the code.
- If the comment fits on one line, add a comment above the line of code. 
- Otherwise, if the comment is longer than one line, use the regular commenting conventions.

Examples:
```
protected const string TAG = "SOOMLA StoreInfo"; //used for Log error messages

// TAG is used for Log error ...messages more-text more-text more-text more-text...
protected const string TAG = "SOOMLA StoreInfo"; 

/// TAG is used for Log error messages ...more-text more-text more-text more-text more-text
/// more-text more-text more-text more-text more-text...
protected const string TAG = "SOOMLA StoreInfo";

```








