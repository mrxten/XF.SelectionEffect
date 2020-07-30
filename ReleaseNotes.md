# Release notes

## 1.7.0
Added the Hold command for both iOS and Android

## 1.6.3
Small internal bugfixes.

## 1.6.2
Fix border view renderer crashes when element is null.

## 1.6.1
Downgrade .NETStandard to 1.0 for compatibility with old PCL projects.

## 1.6.0
  * Add BorderView;
  * Add experimental gesture recognizer for iOS:
    * Touch effect immediately starts when you touch View
    * Cancelling touch effect when View scrolled
  * Fix using and add customization alpha channel in touch color;
  * Fix touch animation ending in old android (in API < 21);
  * Reformat and restruct source code;
  * Add standard sound effect for Android when touch.
  * Changed logic appearing overlay view. Now overlay always is subview for view with touch effect.

## 1.5.6
Small exception bugfixes.

## 1.5.4 
Fix using disposed view, fix using Command.CanExecute, fix style guide.

## 1.5.3
Fix using disposed containers for touch effect

## 1.5.2
Fix long tap for android in XF 3.0+

## 1.5.1
Fix bugs, add support tap through overlapped effect for fast clicks, add auto children transparent

## 1.5.0
Update to .NETStandard 2.0, fix bugs, add EffectsConfig.

## 1.4.0
Update XForms to 2.5.0, fix bug with nesting effects, fix bug with iOS long tap gesture.

## 1.3.3
Stable version for XF 2.3.4
