// JavaScript for Client Details Page and Sidebar Interaction

// Function to load content dynamically
function loadContent(linkElement) {
    const url = linkElement.getAttribute("data-url");
    const dynamicContent = document.getElementById("dynamic-content");

    dynamicContent.innerHTML = "<p>Loading...</p>"; // Show loading indicator

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok " + response.statusText);
            }
            return response.text();
        })
        .then(data => {
            dynamicContent.innerHTML = data; // Update content with response
        })
        .catch(error => {
            dynamicContent.innerHTML = `<p>Error loading content: ${error.message}</p>`;
        });
}

// Automatically load the first sidebar link on page load
document.addEventListener("DOMContentLoaded", () => {
    const defaultLink = document.querySelector("#default-link"); // Target the default link
    if (defaultLink) {
        loadContent(defaultLink); // Trigger content loading for the default link
    }
});
