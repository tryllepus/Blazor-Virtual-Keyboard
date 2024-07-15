using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Serilog;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorVirtualKeyboard.Models;
using System.Net.Http;

namespace BlazorVirtualKeyboard.Services
{
    public class VirtualKeyboardService
    {
        public bool CapsLock { get; set; } = false;

        public bool showPopup { get; set; } = false;

        public Action? _enterKeyPressed { get; set; }

        public Action<string>? OnInputIdChanged { get; set; }

        // We create a dictionary to keep track of values for the input fields
        private Dictionary<string, string> _inputValues = new Dictionary<string, string>();

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public VirtualKeyboardService(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public void SetEnterKeyAction(Action action)
        {
            if (action is not null)
            {
                _enterKeyPressed = action;
            }
            else
            {
                _enterKeyPressed = null;
            }
        }

        public void EnterKeyPressed()
        {
            if (_enterKeyPressed != null)
            {
                _enterKeyPressed?.Invoke();
            }
            else
            {
                Log.Warning($"No function added to key \"Enter\"");
            }
        }


        public void InputIdChanged(string id)
        {
            OnInputIdChanged?.Invoke(id);
        }

        public void SetInputValue(string fieldName, string value = "")
        {
            if (_inputValues.ContainsKey(fieldName))
            {
                _inputValues[fieldName] = value;
            }
            else
            {
                _inputValues.Add(fieldName, value);
            }
        }

        public string GetInputValue(string fieldName)
        {
            if (_inputValues.ContainsKey(fieldName))
            {
                return _inputValues[fieldName];
            }
            Log.Information($"No input field with this name: {fieldName}");
            return "";
        }

        // Method for clearing the inputvalue for a specific field
        public void ClearInputValue(string fieldName)
        {
            if (_inputValues.ContainsKey(fieldName))
            {
                _inputValues[fieldName] = "";
            }
        }

        public MarkupString GetKeyLabel(string key)
        {
            return key switch
            {
                "numbers" => new MarkupString("123"),
                "letters" => new MarkupString("ABC"),
                "globe" => new MarkupString("<i class=\"fa-solid fa-globe\"></i>"),
                "left" => new MarkupString("<i class=\"fa-solid fa-angle-left\"/>"),
                "right" => new MarkupString("<i class=\"fa-solid fa-angle-right\"/>"),
                "backspace" => new MarkupString("<i class=\"fa-solid fa-delete-left\"></i>"),
                "shift" => new MarkupString("<i class=\"fa-solid fa-up-long\"></i>"),
                "enter" => new MarkupString("<i class=\"rotate fa-solid fa-arrow-turn-down\"></i>"),
                "space" => new MarkupString(string.Empty),
                "done" => new MarkupString("Done"),
                _ => CapsLock ? new MarkupString(key.ToUpper()) : new MarkupString(key.ToLower()),
            };
        }
        public string GetKeyClass(string key)
        {
            return key switch
            {
                "shift" or "numbers" or "letters" or "globe" or "left" or "right" => "keyboard__key key-action",
                "backspace" => "keyboard__key keyboard__key--double-wide key-action",
                "enter" => "keyboard__key keyboard__key--wide key-action",
                "space" => "keyboard__key keyboard__key--extra--wide",
                _ => "keyboard__key",
            };
        }

        public async Task InitializeKeyboardEventListeners()
        {
            await JSRuntime.InvokeVoidAsync("listenToKeyboard");
        }

        public async Task<int> GetCursorPosition(string inputId)
        {
            var cursorPositionResult = await JSRuntime.InvokeAsync<int>("getCursorPosition", inputId);
            if (cursorPositionResult < 0)
            {
                Log.Information($"Invalid cursor position returned: {cursorPositionResult}");
                return 0;
            }
            else
            {
                return cursorPositionResult;
            }
        }

        public async Task SetCursorPosition(string inputId, int position)
        {
            await JSRuntime.InvokeVoidAsync("setCursorPosition", inputId, position);
        }

        // Necesarry to update the string in the UI thread
        public async Task SetInputValueJS(string inputId, string newValue)
        {
            await JSRuntime.InvokeVoidAsync("setInputValue", inputId, newValue);
        }
    }
}