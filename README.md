# KeyBindingsEditor
The simple designer editor for the keyboard configuration for games

![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/e3c1abc2-e448-45de-a2be-23b6241dc61b)


# Get started
* Download the [latest release](https://github.com/IOExcept10n/KeyBindingsEditor/releases/)
* Open the application
* Create actions and action categories (categories page)
* Edit actions on keyboard/mouse/gamepad pages

## Usage
### Actions and Categories
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/7647cae6-a5bc-4c8d-a8c6-701e9a845b70)

When you create bindings, you use __actions__. The action defines any gameplay process or any function that can be initiated by user. </br>
The set of similar bindings is called __category__. You can create actions only in categories. The category has a color and an unique name to identify. All actions in the same category have to be the different names.

### Input sources
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/e3cbee1a-df00-49d1-9f24-d705030818b0)

Currently editor supports three input sources: __Keyboard__, __Mouse__ and __Gamepad__. Any input source has a set of keys (buttons), each of them can have bindings on it. </br>
To bind an action to the button, open the input source you want to use and click a button you needed. When you clicked a button, the editor panel below the input source view becomes active.

### Editor panel
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/a73ba421-30f9-4129-9e64-525821070648)

On the editor panel you can select actions for the following default button usage scenarios:
1. Click - the action is handled on the key trigger (press and release);
2. Double Click - the action is handled on the key double-click;
3. Hold - the action is handled when the key is pressed until it gets released.

### Key combinations
On the right side of the actions panel you can edit key combinations. </br>
Each button of three is used to the single part of the combination. The editor currently supports combinations only cross one input source. </br>
When you click the button, the view changes to the list of buttons and their actions according to the previous combination key and you can select the next button for the current combination. </br>
You can make combinations up to three keys.

### Buttons highlight
The buttons can be highlighted according to the actions bent to it in the current context. </br>
When you bind a click action, the button background changes according to the category color. </br>
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/01a62e8b-2856-45c0-93ff-d3a7fb9b3a26) </br>
When you bind a double-click, the button has a circle in its top-right corner. </br>
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/36599dad-3b39-4d66-9a8f-80c8cbc99f76) </br>
When you bind a hold, the button has a circle in its bottom-right corner. </br>
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/5132f49d-ba19-4e07-9688-7bd741b080b9) </br>
When the button is used as the source for the keyboard combination, it has an orange rectangle with the amount of combinations in it. </br>
![изображение](https://github.com/IOExcept10n/KeyBindingsEditor/assets/56886020/7cc6886f-bf1d-4192-b55a-46336fb16b5a) </br>

## Credits
Some icons are taken from the flaticon.com </br>
Special thanks to [@WiverLord](https://github.com/WiverLord) for the input sources layout creation 
