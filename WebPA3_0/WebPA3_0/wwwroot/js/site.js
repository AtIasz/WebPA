function deleteCs(id) {

    console.log("ez az id: " + id);
    var xhr = new XMLHttpRequest();
    xhr.addEventListener("load", init);
    xhr.open("Post", "https://localhost:44370/api/Delete/DeleteJS");


    var data = new FormData();
    data.append("id", id);
    xhr.send(data);


};
function init() {
    const xhr = new XMLHttpRequest();
    xhr.addEventListener("load", onItemsReceived);
    xhr.open("GET", "https://localhost:44370/api/Delete/GetItems");
    xhr.send();
}
function onItemsReceived() {

    const items = JSON.parse(this.responseText);
    var mainDiv = document.getElementById("main");
    var itemTable = document.getElementById("item_table");
    while (itemTable.firstChild) {
        itemTable.removeChild(itemTable.firstChild)
    }
    var hRow = itemTable.insertRow(0);
    var hCell1 = document.createElement("th");
    var hCell2 = document.createElement("th");
    var hCell3 = document.createElement("th");
    hCell1.innerHTML = "Item name";
    hCell2.innerHTML = "Price";
    hCell3.innerHTML = "Sale%";
    hRow.appendChild(hCell1);
    hRow.appendChild(hCell2);
    hRow.appendChild(hCell3);
    items.forEach(q => {
        var index = 1;
        var row = itemTable.insertRow(index);
        var itemName = q.item_name;
        var itemPrice = q.item_price;
        var salePercent = q.sale_percent;
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = document.createElement("td");
        var deleteButton = document.createElement("button");
        deleteButton.innerText = "Delete This";
        deleteButton.onclick = function () {
            deleteCs(q.item_id);

        }
        cell4.appendChild(deleteButton);
        row.appendChild(cell4);
        cell1.innerHTML = itemName;
        cell2.innerHTML = itemPrice;
        cell3.innerHTML = salePercent;
        index = index + 1;
    })





}
init();