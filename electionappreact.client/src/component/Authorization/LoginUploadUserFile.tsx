import React, { useState } from "react";
import { generatePkcePair } from "../../utils/pkce";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import { API_BASE } from "../../config/api";

//const API_BASE = "https://localhost:7127";

const LoginUploadUserFile: React.FC = () => {
    const [bank, setBank] = useState("");
    const [password, setPassword] = useState("");
    const [fileData, setFileData] = useState<any>(null);

    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    // тільки зчитує файл
    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = () => {
            try {
                const text = reader.result as string;
                const json = JSON.parse(text);
                setFileData(json);
            } catch {
                setError("JSON файл некоректний");
            }
        };
        reader.readAsText(file);
    };

    const handleLogin = async () => {
        setError("");
        setLoading(true);

        if (!bank || !password || !fileData) {
            setError("Заповніть всі поля");
            setLoading(false);
            return;
        }

        // додаємо банк і пароль у JSON
        const payload = {
            ...fileData,
            Bank: bank,
            Password: password
        };

        // 1) бекенд авторизація
        const resp = await fetch(`${API_BASE}/api/authorization/check`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
        });

        if (!resp.ok) {
            setError("Невірні дані");
            setLoading(false);
            return;
        }
        const data = await resp.json();
        localStorage.setItem("user", JSON.stringify(fileData));
        localStorage.setItem("role", data.status);

        // 2) PKCE
        const { verifier, challenge } = await generatePkcePair();
        sessionStorage.setItem("pkce_verifier", verifier);

        const loginResp = await fetch(
            `${API_BASE}/api/authorization/login-url?codeChallenge=${encodeURIComponent(challenge)}`
        );

        if (!loginResp.ok) {
            setError("Помилка авторизації");
            setLoading(false);
            return;
        }

        const url = await loginResp.text();
        window.location.href = url;
    };

    return (
        <div className="form-container">

            <Form.Select
                className="mb-4"
                value={bank}
                onChange={(e) => setBank(e.target.value)}
            >
                <option value="">Вибрати банк</option>
                <option value="privat24">ПриватБанк</option>
                <option value="monobank">Монобанк</option>
                <option value="oschad">Ощадбанк</option>
                <option value="universal">Універсальний</option>
                <option value="admin">Адмін</option>
            </Form.Select>

            <Form.Control
                className="mb-4"
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <Form.Control
                className="mb-4"
                type="file"
                accept=".json"
                onChange={handleFileChange}
            />

            <Button onClick={handleLogin} disabled={loading}>
                Увійти
            </Button>

            {loading && <p>Перевіряю...</p>}
            {error && <p style={{ color: "red" }}>{error}</p>}
        </div>
    );
};

export default LoginUploadUserFile;
