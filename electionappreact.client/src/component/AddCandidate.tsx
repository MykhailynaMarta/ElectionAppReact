import { useState, ChangeEvent, FormEvent } from "react";
import { Form, Row, Col, InputGroup, Button } from "react-bootstrap";

const API_CANDIDATES = "http://localhost:5209/api/candidates";
const API_UPLOAD = "http://localhost:5209/api/upload";

interface CandidateCreate {
    name: string;
    party: string;
    description: string;
    birthdate: string;
    photourl: string;
}

function AddCandidate() {
    const [validated, setValidated] = useState<boolean>(false);
    const [file, setFile] = useState<File | null>(null);

    const [candidate, setCandidate] = useState<CandidateCreate>({
        name: "",
        party: "",
        description: "",
        birthdate: "",
        photourl: "",
    });

    const updateField = (field: keyof CandidateCreate) =>
        (e: ChangeEvent<HTMLInputElement>) => {
            setCandidate({ ...candidate, [field]: e.target.value });
        };

    // ---------- Зчитати файл ----------
    const handleFile = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            setFile(e.target.files[0]);
        }
    };

    // ---------- Submit ----------
    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const form = event.currentTarget;

        if (!form.checkValidity()) {
            event.stopPropagation();
            setValidated(true);
            return;
        }

        let uploadedUrl = "";

        // ---------- 1) Upload photo ----------
        if (file) {
            const formData = new FormData();
            formData.append("file", file);

            const uploadRes = await fetch(API_UPLOAD, {
                method: "POST",
                body: formData,
            });

            const uploadData = await uploadRes.json();
            uploadedUrl = uploadData.url;
        }

        // ---------- 2) Create candidate ----------
        const res = await fetch(API_CANDIDATES, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                ...candidate,
                birthdate: new Date(candidate.birthdate).toISOString(),
                photourl: uploadedUrl,
            }),
        });

        if (res.ok) {
            alert("Кандидат успішно доданий!");
            setValidated(false);
        } else {
            alert("Помилка при збереженні кандидата!");
        }
    };

    return (
        <Form noValidate validated={validated} onSubmit={handleSubmit}>
            <Row className="mb-3">

                <Form.Group as={Col} md="4">
                    <Form.Label>ПІБ</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.name}
                        onChange={updateField("name")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="4">
                    <Form.Label>Партія</Form.Label>
                    <InputGroup hasValidation>
                        <Form.Control
                            required
                            type="text"
                            value={candidate.party}
                            onChange={updateField("party")}
                        />
                        <Form.Control.Feedback type="invalid">
                            Додайте партію!
                        </Form.Control.Feedback>
                    </InputGroup>
                </Form.Group>

            </Row>

            <Row className="mb-3">

                <Form.Group as={Col} md="6">
                    <Form.Label>Дата народження</Form.Label>
                    <Form.Control
                        required
                        type="date"
                        value={candidate.birthdate}
                        onChange={updateField("birthdate")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="3">
                    <Form.Label>Біографія</Form.Label>
                    <Form.Control
                        required
                        type="text"
                        value={candidate.description}
                        onChange={updateField("description")}
                    />
                </Form.Group>

                <Form.Group as={Col} md="3">
                    <Form.Label>Фото кандидата</Form.Label>
                    <Form.Control
                        required
                        type="file"
                        accept="image/*"
                        onChange={handleFile}
                    />
                    <Form.Control.Feedback type="invalid">
                        Завантажте фото.
                    </Form.Control.Feedback>
                </Form.Group>

            </Row>

            <Button type="submit">Додати</Button>
        </Form>
    );
}

export default AddCandidate;
