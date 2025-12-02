import React, { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Button from "react-bootstrap/Button";
import { API_BASE } from "../config/api";

//const API_BASE = "https://localhost:7127";
interface Candidate {
    id: number;
    name: string;
    party: string;
    birthdate: string;
    photourl: string;
    description: string;
}

const Candidates: React.FC = () => {
    const navigate = useNavigate();
    const [search, setSearch] = useState("");

    const [candidates, setCandidates] = useState<Candidate[]>([]);
    const role = localStorage.getItem("role"); // "admin-valid" або "valid"

    useEffect(() => {
        loadCandidates();
    }, []);

    const loadCandidates = async () => {
        const resp = await fetch(`${API_BASE}/api/candidates`);
        const data = await resp.json();
        setCandidates(data);
    };

    const deleteCandidate = async (id: number) => {
        if (!window.confirm("Видалити кандидата?")) return;

        await fetch(`${API_BASE}/api/candidates/${id}`, {
            method: "DELETE",
        });

        setCandidates(candidates.filter(c => c.id !== id));
    };
    const filtered = candidates.filter(c =>
        c.name.toLowerCase().includes(search.toLowerCase()) ||
        c.party.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="w-full max-w-6xl mx-auto py-8 px-4">
            <div className="mb-6">
                <input
                    type="text"
                    placeholder="Пошук по імені або партії..."
                    className="w-full p-3 border rounded-lg"
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            {/* Додати кандидата */}
            {role === "admin-valid" && (
                <div className="mb-6">
                    <Link to={`/candidates/add/`}>
                        <Button variant="">Додати кандидата</Button>
                    </Link>
                </div>
            )}

            {/* Список кандидатів */}
            <div className="space-y-8">
                {filtered.map(candidate => (
                    <div
                        key={candidate.id}
                        className="flex gap-6 p-5 bg-white shadow rounded-xl"
                    >
                        {/* Фото */}
                        <div className="w-40 h-52 bg-gray-200 rounded overflow-hidden flex-shrink-0">
                            <img
                                src={candidate.photourl || "/placeholder.png"}
                                alt="candidate"
                                className="w-full h-full object-cover"
                            />
                        </div>

                        {/* Текстова частина */}
                        <div className="flex flex-col justify-between flex-grow">
                            <div>
                                <h2 className="text-xl font-bold">{candidate.name}</h2>
                                <p className="text-gray-700">{candidate.party}</p>

                                <p className="mt-2 text-sm">
                                    <span className="font-semibold">Дата народження:</span>{" "}
                                    {candidate.birthdate}
                                </p>

                                <button
                                    className="mt-3 text-blue-600 hover:underline"
                                    onClick={() => navigate(`/candidate/${candidate.id}`)}
                                >
                                    Детальніше →
                                </button>
                            </div>

                            {/* Адмінські кнопки */}
                            {role === "admin-valid" && (
                                <div className="flex gap-3 mt-4">
                                    <Link to={`/candidates/edit/${candidate.id}`}>
                                        <Button variant="warning">Редагувати</Button>
                                    </Link>
                                    <button
                                        className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 transition"
                                        onClick={() => deleteCandidate(candidate.id)}
                                    >
                                        Видалити
                                    </button>
                                </div>
                            )}
                        </div>
                    </div>
                ))}
            </div>

        </div>
    );
};

export default Candidates;
