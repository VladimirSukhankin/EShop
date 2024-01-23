let el = document.getElementById("codeClick");
el.addEventListener("click", () => {
    if (window.getSelection) {
        var range = document.createRange();
        range.selectNode(el);
        window.getSelection().removeAllRanges();
        window.getSelection().addRange(range);
    }
});

