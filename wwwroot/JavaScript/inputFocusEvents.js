window.registerFocusEvents = function (dotNetHelper, inputId) {
    const input = document.querySelector(`input#${inputId}.use-keyboard`);
    const keyboard = document.getElementById('keyboard');
    let currentFocusedInputId = null;

    input.addEventListener('focus', () => {

        // We only invoke the dotnet method if the ID of the focused element has changed
        if (currentFocusedInputId != input.id) {
            currentFocusedInputId = input.id;
            dotNetHelper.invokeMethodAsync('InputHasFocus', input.id);
        }
    });

    input.addEventListener('focusout', function (event) {
        const isFocusMovingToKeyboard = keyboard.contains(event.relatedTarget);

        if (!isFocusMovingToKeyboard) {
            dotNetHelper.invokeMethodAsync('InputLostFocus', currentFocusedInputId);
            currentFocusedInputId = null;
        }
        else {
            input.focus();
        }
    });
}