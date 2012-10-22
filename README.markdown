Android.Dialog
================

NOTE: This project is an attempt to port the ideas and ease of use of the 
MonoTouch.Dialog project to Mono for Android.  Given the significant differences 
in the layout and UI architectures between iOS and Android, some things will 
inevitably be different between the projects but conceptually Android.Dialog
should stay very close to MonoTouch.Dialog. Expect lots of changes and 
chaos as more and more elements get added.

Android.Dialog is a foundation to create dialog boxes and show table-based 
information without having to write dozens of delegates and controllers for 
the user interface.

To use this library, reference it from the project that you want to use it from,
and add these layouts to your Resources/Layout folder: [Android.Dialog Layouts](DialogLayouts)

This is expected to be fixed after Mono for Android 4.2.8 is released. ([4.2.8 Release Notes](http://docs.xamarin.com/android/Releases/Mono_For_Android_4/Mono_for_Android_4.2.8))

Pic of the Sample App ([located here](DialogSampleApp))
---------------------
![Rendering of SampleApp](DialogSampleApp/raw/master/sample.png)

Supported versions of Android
----------------
- Android 1.6-4.1
- ARM/x86
- NOT Google TV ([Issue on Google Code](http://code.google.com/p/googletv-issues/issues/detail?id=12))