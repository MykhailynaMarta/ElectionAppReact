import Nav from 'react-bootstrap/Nav';
import { Link, useNavigate } from "react-router-dom";

function Navbar() {
    const navigate = useNavigate();

    return (
        <Nav className="mb-10" variant="underline" defaultActiveKey="/">
            <Nav.Item>
                <Nav.Link as={Link} to="/">Головна</Nav.Link>
            </Nav.Item>

            <Nav.Item>
                <Nav.Link as="button" onClick={() => {
                    const role = localStorage.getItem("role");
                    if (!role) {
                        window.location.href = "/upload";
                    } else {
                        window.location.href = "/user-info";
                    }
                }}>Мій профіль</Nav.Link>
            </Nav.Item>

            <Nav.Item>
                <Nav.Link as={Link} to="/candidates">Кандидати</Nav.Link>
            </Nav.Item>

            <Nav.Item>
                <Nav.Link
                    as="button"
                    className="bg-transparent border-0"
                    onClick={() => {
                        const role = localStorage.getItem("role");
                        if (!role) {
                            window.location.href = "/upload";
                        } else {
                            window.location.href = "/candidates";
                        }
                    }}
                >
                    Голосування
                </Nav.Link>
            </Nav.Item>
        </Nav>
    );
}

export default Navbar;
