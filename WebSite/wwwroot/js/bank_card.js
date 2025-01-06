function toggleTransactions(element) {
    const cardId = element.dataset.cardId; // Получаем cardId из data-card-id
    const transactionRow = document.getElementById(`transactions-${cardId}`);
    if (transactionRow.style.display === "none") {
        transactionRow.style.display = ""; // Показываем строку с транзакциями
        loadTransactions(cardId); // Загружаем транзакции через AJAX
    } else {
        transactionRow.style.display = "none"; // Скрываем строку с транзакциями
    }
}

async function loadTransactions(cardId) {
    const tableBody = document.getElementById(`transaction-list-${cardId}`);
    tableBody.innerHTML = "<p>Loading transactions...</p>";

    try {
        const response = await fetch(`/BankCard/GetTransactions?cardId=${cardId}`);
        if (response.ok) {
            const transactions = await response.json();
            if (transactions.length === 0) {
                tableBody.innerHTML = "<p>No transactions found.</p>";
            } else {
                tableBody.innerHTML = transactions.map(transaction => `
                    <div>
                        <p>${new Date(transaction.date).toLocaleDateString()} - ${transaction.quantity} - ${transaction.actionType === 1 ? 'Income' : 'Expense'} - ${transaction.balance}</p>
                    </div>
                `).join('');
            }
        } else {
            tableBody.innerHTML = "<p>Error loading transactions.</p>";
        }
    } catch (error) {
        console.error(error);
        tableBody.innerHTML = "<p>Error loading transactions.</p>";
    }
}
