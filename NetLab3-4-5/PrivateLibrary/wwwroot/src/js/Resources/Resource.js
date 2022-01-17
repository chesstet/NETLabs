$(document).ready(function() {

    document.getElementById("generate-token").onclick = async () => {
        const response = await fetch('/Resources/GetToken',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json;charset=utf-8',
                    'Accept': 'application/json;charset=utf-8'
                }
            });
        if (response.ok === true) {
            const result = await response.json();
            document.getElementById("private-token").value = result.token.toString();
            SuccessMessage("A new token has been got!", 5000);
        } else {
            ErrorMessage("Cannot renew a token!", 5000);
        }
    }

});