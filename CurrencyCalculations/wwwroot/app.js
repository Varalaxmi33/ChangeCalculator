function calculate() {

    var amount = document.getElementById("amount").value;
    var price = document.getElementById("price").value;
    var result = document.getElementById("result");

    result.innerHTML = "";

    if (amount === "" || price === "") {
        alert("Please enter both values.");
        return;
    }

    var request = {
        amountGiven: parseFloat(amount),
        productPrice: parseFloat(price)
    };

    var xhr = new XMLHttpRequest();

    xhr.open("POST", "/api/change", true);
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {

        if (xhr.readyState === 4) {

            if (xhr.status === 200) {

                var data = JSON.parse(xhr.responseText);

                if (data.length === 0) {
                    result.innerHTML = "<h3>No Change</h3>";
                    return;
                }

                var html = "<h3>Your Change</h3>";
                html += "<table border='1' cellpadding='8' cellspacing='0'>";
                html += "<tr><th>Denomination</th><th>Count</th></tr>";

                for (var i = 0; i < data.length; i++) {

                    html += "<tr>";
                    html += "<td>" + data[i].denomination + "</td>";
                    html += "<td>" + data[i].count + "</td>";
                    html += "</tr>";
                }

                html += "</table>";

                result.innerHTML = html;
            }
            else {
                alert(xhr.responseText);
            }
        }
    };

    xhr.send(JSON.stringify(request));
}