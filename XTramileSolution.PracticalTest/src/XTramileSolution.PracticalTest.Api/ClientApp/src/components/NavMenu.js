import React, { useState } from 'react';
import { Container, Navbar, Nav } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

const NavMenu = () => {
    const [expanded, setExpanded] = useState(false);

    return (
        <header>
            <Navbar expand="sm" bg="light" variant="light" className="border-bottom shadow-sm mb-3" expanded={expanded}>
                <Container>
                    <Navbar.Brand as={Link} to="/">XTramile Solution Weather</Navbar.Brand>
                    <Navbar.Toggle onClick={() => setExpanded(!expanded)} />
                    <Navbar.Collapse className="justify-content-end">
                        <Nav>
                            <Nav.Link as={Link} to="/" onClick={() => setExpanded(false)}>
                                Home
                            </Nav.Link>
                        </Nav>
                    </Navbar.Collapse>

                </Container>
            </Navbar>
        </header>
    );
};

export default NavMenu;