// src/pages/CallbackPage.tsx
import { useEffect } from "react";
import { API_BASE } from "../../config/api";

//const API_BASE = "https://localhost:7127"; // ElectionAppServer

export default function CallbackPage() {
    useEffect(() => {
        const run = async () => {

            const url = new URL(window.location.href);
            const code = url.searchParams.get("code");
            const verifier = sessionStorage.getItem("pkce_verifier");

            if (!code || !verifier) {
                console.error("Missing code or verifier");
                window.location.href = "/";
                return;
            }

            //  1. Міняємо code на tokens через ElectionAppServer
            const resp = await fetch(`${API_BASE}/api/oauth/callback`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ code, verifier })
            });

            if (!resp.ok) {
                const txt = await resp.text();
                console.error("Callback exchange failed", txt);
                window.location.href = "/";
                return;
            }

            //  2. Отримуємо токени
            const raw = await resp.text();  // сервер повертає JSON-рядок
            const tokens = JSON.parse(raw);

            localStorage.setItem("access_token", tokens.access_token);
            sessionStorage.removeItem("pkce_verifier");

            //  3. Перенаправляємо користувача
            window.location.href = "/candidates";
        };

        run();
    }, []);

    return <p>Processing login...</p>;
}
