# Blazor-Virtual-Keyboard
A virtual keyboard / on-screen keyboard for Blazor WebAssembly .NET8, using minimal JavaScript

Currently only supports Blazor Webassembly on .NET 8 - support for Blazor Server is progressing.

NOTE: This is simply a proof of concept/prototype, and therefore does not support all use cases. As JavaScript and Blazor has to notify each other of changes and such, I have created Blazor components called FormVirtualKeyboard.razor and InputVirtualKeyboard.razor which are the only components that support integration of this keyboard.

![Alphabetic keyboard](https://i.imgur.com/a/A15qSeE)

I have tried to minimize the amount of JavaScript in the implementation of this virtual keyboard / on-screen keyboard component. This has resulted in some use cases not being supported, and some atypical conventions in the code. Only minimum refactoring has been done as of now, which might be why there can (will) be code smells and minor bugs.

TBD:

Keyboard customization
### INSTALLATION

Install this NuGet package in your Blazor Webassembly project on .NET 8.

Add the following lines to your program.cs: 
```csharp
builder.Services.AddHttpClient(); 
builder.Services.AddScoped<BlazorVirtualKeyboard.Services.VirtualKeyboardService>();
```

Add 
```csharp
@using BlazorVirtualKeyboard 
```
to your _Imports.razor file.

Add the following lines to your index.html: 
```html
<script src="_content/BlazorVirtualKeyboard/Components/InputVirtualKeyboard.razor.js"></script> 
<script src="_content/BlazorVirtualKeyboard/Components/FormVirtualKeyboard.razor.js"></script> 
<script src="_content/BlazorVirtualKeyboard/Keyboards/VirtualKeyboard.razor.js"></script>
```

(Optional but recommended) Add the following line to index.html if you want icons for the function buttons: 
```html
<script src="https://kit.fontawesome.com/be5149d719.js" crossorigin="anonymous"></script>
```

### USAGE
As mentioned, the use cases might differ from other on-screen keyboard projects since the goal of this projects was to use as little JavaScript as possible. To use the keyboard, put
```html
<div>
    <div>
     ... 
    </div> 
    <BlazorVirtualKeyboard.Keyboards.VirtualKeyboard /> 
</div>
```

In your MainLayout.razor, this is the only instance of VirtualKeyboard that is necessary, as it is merely hidden and listens for focus events on the custom components that support it. Here is an example of how to combine FormVirtualKeyboard and InputVirtualKeyboard for submitting data using the keyboard: 
```html
<FormVirtualKeyboard FormAction="Login" FormId="loginForm" FormClass="login-form row-1 test-form" EditContext="loginEditContext">

<h5>login</h5>
<div class="input">
    <label for="username" />
    <InputVirtualkeyboard @bind-InputValue="LoginModelInstance.Username"
                            Id="username" Type="text" Placeholder="username" />

    <label for="password" />
    <InputVirtualkeyboard @bind-InputValue="LoginModelInstance.Password"
                            Id="password" Type="password" Placeholder="password" />
</div>
<div class="action">
    <button type="submit" class="bluebtn">
        Login
    </button>
</div>
</FormVirtualKeyboard>

@code {
    public LoginModel? LoginModelInstance = new LoginModel();
    private EditContext? loginEditContext;

    protected override async Task OnInitializedAsync()
    {
        loginEditContext = new EditContext(LoginModelInstance);
    }

    private void Login()
    {
        if (loginEditContext.Validate())
        {
            Log.Information($"User: {LoginModelInstance?.Username}, Password: {LoginModelInstance?.Password}");
        }
    }

    public class LoginModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username is too short.")]
        public string? Username { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Password is too short.")]
        public string? Password { get; set; }
    }
}
```
It is also possible to just use InputVirtualKeyboard if you do not wish to submit any data:
```html
<BlazorVirtualKeyboard.Components.InputVirtualkeyboard Placeholder="test" Id="test" Type="text"/>
```
