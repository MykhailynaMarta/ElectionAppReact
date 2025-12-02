function UserInfo() {
    const token = localStorage.getItem("access_token");

    if (!token) {
        return <p>Ви не авторизовані.</p>;
    }

    const raw = localStorage.getItem("user");
    if (!raw) {
        return <p>Дані користувача не знайдені.</p>;
    }

    const user = JSON.parse(raw);

    return (
        <div>
            <h2>Інформація про користувача</h2>
            <p><strong>ПІБ:</strong> {user.fullname}</p>
            <p><strong>Банк:</strong> {user.bank}</p>
            <p><strong>Дата народження:</strong> {user.birthdate}</p>
            <p><strong>Адреса:</strong> {user.address}</p>
        </div>
    );
}


export default UserInfo;
