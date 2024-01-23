let el = document.getElementById("code");
el.value = null;
el.addEventListener("input", () => {
    if (el.value.length == 4) {
        document.getElementById("codeForm").submit();
    }
});
el.focus();
