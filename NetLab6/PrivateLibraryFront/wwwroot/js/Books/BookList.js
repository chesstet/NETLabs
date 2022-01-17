const url = "https://localhost:7131/api/v1";
const currentSiteUri = "https://localhost:7124";

$(document).ready(function () {
    $("#filterForm").submit(function (event) {
        event.preventDefault();

        const firstName = $("#firstName").val();
        const lastName = $("#lastName").val();
        const bookName = $("#bookName").val();
        const bookYear = $("#bookYear").val();
        const direction = $("#direction").val();

        $.get(url + "/Book/GetByFilter",
                {
                    authorFirstName: firstName,
                    authorLastName: lastName,
                    bookName: bookName,
                    bookYear: bookYear,
                    directionName: direction
                })
            .done(function(data) {
                let books = data.entities;
                clearAllBooksData();
                insertBooksData(books);
            })
            .fail(function() {
                alert("error");
            });
    });

    $(document).on('click', ".openBook",function (e) {
        e.preventDefault();
        let dataId = $(this).parents(".book-block").data("id");
        window.location.replace(currentSiteUri + "/Book/Get?id=" + dataId);
    });

    function clearAllBooksData() {
        $("#bookList").empty();
    }

    function insertBooksData(books) {
        $.each(books, function (index, value) {
            $('#bookList').append(buildBookInstance(value.id, value.name, value.authorName, value.directionName));
        });
    }

    function buildBookInstance(id, name, authorName, directionName) {
        let block =
            '<div class="card h-100 book-block" data-id="' + id + '"> <img src="https://loremflickr.com/320/240" class="card-img-top" alt="..."> <div class="card-body"> <h5 class="card-title">' + name + '</h5> <p class="card-text">' + directionName + '</p> <p class="card-text"><small class="text-muted">' + authorName + '</small></p> </div>' +
            '<div class="card-footer"> <a class="btn btn-success openBook" role="button" >Open</a> <form method="post"> <button type="submit" class="btn btn-primary addToCollection" >Add to MyCollection</button> </form> </div> </div>';
        return block;
    }
});




//$.ajax({
//    url: url,
//    type: "GET",
//    data: JSON.stringify(data),
//    contentType: "application/json; charset=utf-8",
//    dataType: "json"
//}).done(function (data) {
//    alert(data);
//});


//$.get("https://localhost:44339/weatherforecast", function (data) {
 //   })
    //.done(function (data) {
    //    alert(data);
    //})
    //.fail(function () {
    //    alert("error");
    //})
    //.always(function () {
    //    alert("finished");
    //});


//$.ajax({
//    url: url,
//    type: "POST",
//    data: { s: term },
//    contentType: "application/json; charset=utf-8",
//    dataType: "json"
//}).done(function (data) {
//    alert(data);
//});