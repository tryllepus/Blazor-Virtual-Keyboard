function showKeyboard() {
    const keyboard = document.getElementById('keyboard');
    keyboard.classList.remove('keyboard--hidden');
    keyboard.classList.add('keyboard--shown');
}

function hideKeyboard() {
    const keyboard = document.getElementById('keyboard');
    keyboard.classList.remove('keyboard--shown');
    keyboard.classList.add('keyboard--hidden');
}

//Global click listener to handle clicks outside the keyboard
window.listenToKeyboard = function () {
    const keyboard = document.getElementById('keyboard');
    let showingKeyboard = false;

    document.addEventListener('focus', function (event) {
        if (event.target.tagName === 'INPUT' && event.target.classList.contains('use-keyboard')) {
            if (!showingKeyboard) {
                showKeyboard();   
                showingKeyboard = true;
            }
        }
    }, true);

    document.addEventListener('focusout', function (event) {
        const isInput = event.target.tagName === 'INPUT';
        // Check if the relatedTarget (element gaining focus) is not the keyboard or a child of it.
        const isFocusMovingToKeyboard = keyboard.contains(event.relatedTarget);

        // If focus is lost from an input and not moving to the keyboard, hide the keyboard.
        if (isInput && !isFocusMovingToKeyboard) {
            hideKeyboard();
            showingKeyboard = false;
        }
    }, true);
}

window.updateInputValue = function (value, id) {
    if (document.getElementById('keyboard').classList.contains('keyboard--shown')) {
        try {

            const input = document.querySelector('input#' + id);

            if (input) {
                // This is needed to update the input value in the UI
                setInputValue(input.id, value)
            }
            else {
                console.log('Cannot update value for non-existing input field');
            }
        }
        catch (error) {
            if (id === "" || id === null) {
                console.warn(`caught error trying the select input with no id: ${error}`);
            }
            else {
                console.warn(`caught error trying the select input with id ${id}: ${error}`);
            }
        }
    }
}

window.getCursorPosition = function (inputId) {
    const input = document.getElementById(inputId);

    if (input && typeof input.selectionStart === "number") {
        return input.selectionStart;
    }

    return -1;
}

window.setCursorPosition = function (inputId, position) {
    const input = document.getElementById(inputId);

    if (!input) {
        console.error('Element not found: ' + inputId);
        return;
    }
    if (typeof input.setSelectionRange !== 'function') {
        console.error('setSelectionRange is not supported on the element: ' + inputId);
        return;
    }

    input.setSelectionRange(position, position);
    input.focus();
}

window.setInputValue = function (inputId, value) {
    const input = document.getElementById(inputId);

    if (input) {
        input.value = value; // Set the value first
        input.dispatchEvent(new Event('input', { bubbles: true })); // Ensure Blazor updates the bound value
    }
}