import { useState, useEffect, ChangeEvent, FormEvent } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, Row, Col, InputGroup, Button } from "react-bootstrap";

const API_BASE = "http://localhost:5209/api/candidates";

interface Candidate {
    id: number;
    name: string;
    party: string;
    description: string;
    birthdate: string;
    photourl: string;
}

function UpdateCandidate() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [candidate, setCandidate] = useState<Candidate>({
        id: 0,
        name: "",
        party: "",
        description: "",
        birthdate: "",
        photourl: "",
    });

    const [validated, setValidated] = useState<boolean>(false);

    // -------- 1. Load candidate --------
    useEffect(() => {
        const load = async () => {
            const res = await fetch(`${API_BASE}/${id}`);
            const data: Candidate = await res.json();

            setCandidate({
                ...data,
                birthdate: data.birthdate.substring(0, 10), // only yyyy-mm-dd
            });
        };

        load();
    }, [id]);

    // -------- 2. Update --------
    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const res = await fetch(`${API_BASE}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(candidate),
        });

        if (res.ok) {
            alert("Кандидата оновлено успішно!");
            navigate("/candidates");
        } else {
            alert("Помилка при оновленні!");
        }
    };

    const handleInput =
        (field: keyof Candidate) =>
            (e: ChangeEvent<HTMLInputElement>) => {
                setCandidate({ ...candidate, [field]: e.target.value });
            };

    return (
        <Form noValidate validated={validated} onSubmit={handleSubmit}>
            <h3 className="mb-3">Редагування кандидата</h3>

            <Row className="mb-3">

                <Form.Group as={Col} md="4">
                    <Form.Label>ПІБ</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.name}
                        onChange={handleInput("name")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="4">
                    <Form.Label>Партія</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.party}
                        onChange={handleInput("party")}
                    />
                </Form.Group>

            </Row>

            <Row className="mb-3">

                <Form.Group as={Col} md="6">
                    <Form.Label>Дата народження</Form.Label>
                    <Form.Control
                        required
                        type="date"
                        value={candidate.birthdate}
                        onChange={handleInput("birthdate")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="3">
                    <Form.Label>Біографія</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.description}
                        onChange={handleInput("description")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="3">
                    <Form.Label>Фото URL</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.photourl}
                        onChange={handleInput("photourl")}
                    />
                </Form.Group>

            </Row>

            <Button type="submit">Оновити</Button>
        </Form>
    );
}

export default UpdateCandidate;
