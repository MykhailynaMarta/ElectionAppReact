import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
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

const CandidateDetails: React.FC = () => {
    const { id } = useParams();
    const [candidate, setCandidate] = useState<Candidate | null>(null);

    useEffect(() => {
        fetchCandidate();
    }, []);

    const fetchCandidate = async () => {
        const resp = await fetch(`${API_BASE}/api/candidates/${id}`);
        const decoder = new TextDecoder("utf-8");
        const text = decoder.decode(await resp.arrayBuffer());
        const data = JSON.parse(text);
        setCandidate(data);
    };

    if (!candidate) return <p className="text-center mt-10">Завантаження...</p>;

    return (
        <div className="max-w-4xl mx-auto p-6">

            {/* Фото */}
            <div className="flex justify-center">
                <img
                    src={candidate.photourl || "/placeholder.png"}
                    alt="candidate"
                    className="w-full h-full object-cover"
                />
            </div>

            {/* Інформація */}
            <h1 className="text-3xl font-bold mt-6 text-center">{candidate.name}</h1>
            <p className="text-xl text-gray-700 text-center">{candidate.party}</p>

            <p className="mt-4 text-lg">
                <span className="font-semibold">Дата народження:</span>{" "}
                {candidate.birthdate}
            </p>

            {/* Опис */}
            <h2 className="text-2xl font-semibold mt-6">Біографія</h2>
            <p className="mt-2 leading-relaxed text-gray-800">
                {candidate.description}
            </p>

        </div>
    );
};

export default CandidateDetails;
