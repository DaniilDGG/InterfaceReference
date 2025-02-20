# Interface Reference

## Overview

**Interface Reference** is a Unity package designed to facilitate the serialization of references to interfaces. This package is particularly useful when you need to store references to components that implement specific interfaces, rather than concrete classes, in Unity's inspector.

## Features

- Serialize references to interfaces in Unity.
- Easily select objects implementing a specific interface using a custom editor window.
- Supports both GameObjects and ScriptableObjects.

## Installation

### Using Unity Package Manager (UPM)

To install the package via Unity Package Manager, follow these steps:

1. Open your Unity project.
2. Go to `Window` > `Package Manager`.
3. Click on the `+` button in the top left corner and select `Add package from git URL...`.
4. Enter the following URL:

   ```
   https://github.com/DaniilDGG/InterfaceReference.git
   ```

5. Click `Add` to install the package.

## Usage

### InterfaceReference Class

The `InterfaceReference<T>` class allows you to serialize references to interfaces. Here's a basic example of how to use it:

```csharp
using InterfaceReference;

public class Example : MonoBehaviour
{
    [SerializeField] private InterfaceReference<IMyInterface> _interfaceReference;

    private void Start()
    {
        if (_interfaceReference.IsValid)
        {
            IMyInterface myInterface = _interfaceReference.Value;
            myInterface.DoSomething();
        }
    }
}
```

### Interface Picker Window

The package includes a custom editor window for selecting objects that implement a specific interface. This window can be opened programmatically:

```csharp
InterfacePickerWindow.ShowPicker(typeof(IMyInterface), selectedObject =>
{
    // Handle the selected object
});
```

## License

This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.
