window.registerFormFocusEvents = function (dotNetHelper, formId) {
    const form = document.querySelector(`form#${formId}`);
    const keyboard = document.getElementById('keyboard');
/*    let currentFocusedFormId = null;*/

    form.addEventListener('focusin', () => {
        // We only invoke the dotnet method if the ID of the focused element has changed
        if (!form.hasFocus) {
            dotNetHelper.invokeMethodAsync('FormHasFocus', form.id);
            form.hasFocus = true;
        }
    });

    form.addEventListener('focusout', function (event) {
        const isFocusMovingToKeyboard = keyboard.contains(event.relatedTarget);

        if (!isFocusMovingToKeyboard) {
            dotNetHelper.invokeMethodAsync('FormLostFocus', form.id);
            form.hasFocus = false;
        }
        else {
            form.focus();
        }
    });
}