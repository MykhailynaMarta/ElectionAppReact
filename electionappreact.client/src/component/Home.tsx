import { Link } from "react-router-dom";

function Home() {
    return (
        <section className="">
            <h2>Welcome to the ElectionApp!</h2>

            <p className="mt-3 text-lg">
                Цей додаток створений для проведення онлайн-голосування.
                Він дозволяє користувачам переглядати список кандидатів,
                знайомитися з їхньою біографією, а після авторизації —
                брати участь у голосуванні. Адміністратори можуть додавати,
                редагувати та видаляти кандидатів.
            </p>

            <p className="mt-4">
                Щоб перейти до голосування, будь ласка, авторизуйтесь →{" "}
                <Link to="/upload" className="text-blue-600 underline">
                    Увійти
                </Link>
            </p>
        </section>
    );
}

export default Home;
