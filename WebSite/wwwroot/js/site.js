// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Target only the sidebar links, assuming they are wrapped in a specific container with an ID like 'sidebar'
    $("#sidebar .nav-link").on("click", function (e) {
        e.preventDefault(); // Disable default link behavior

        const url = $(this).attr("href"); // Get URL from href attribute
        const clientId = $("#dynamic-content").data("client-id"); // Get client ID from data attribute

        $("#dynamic-content").html("<p>Loading...</p>"); // Show loading indicator

        $.get(url, { clientId: clientId }) // Send GET request with clientId parameter
            .done(function (data) {
                $("#dynamic-content").html(data); // Load content into dynamic-content div
            })
            .fail(function () {
                $("#dynamic-content").html("<p>Error loading content. Please try again.</p>");
            });
    });
});
function loadContent(linkElement) {
    const url = linkElement.getAttribute("data-url");
    const dynamicContent = document.getElementById("dynamic-content");

    dynamicContent.innerHTML = "<p>Loading...</p>";

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok " + response.statusText);
            }
            return response.text();
        })
        .then(data => {
            dynamicContent.innerHTML = data;
        })
        .catch(error => {
            dynamicContent.innerHTML = `<p>Error loading content: ${error.message}</p>`;
        });
}
