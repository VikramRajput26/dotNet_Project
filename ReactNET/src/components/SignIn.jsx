import React, { useState } from "react";
import { Form, Button, Container, Alert } from "react-bootstrap";
import { login } from "../services/userService";
import { toast, ToastContainer } from "react-toastify";
import { useNavigate } from "react-router-dom";
import "./SignIn.css";
import "react-toastify/dist/ReactToastify.css";

function SignIn() {
  const [Username, setUsername] = useState("");
  const [Password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const loginRequest = { Username, Password }; // Create the request object with matching field names
      const { token, userId } = await login(loginRequest);
      localStorage.setItem("token", token);
      localStorage.setItem("userId", userId);
      setError("");
      toast.success("Login successful!");
      navigate("/childregister");
    } catch (error) {
      setError(error.message);
      toast.error("Login failed: " + error.message);
    }
  };

  const handleBackToHome = () => {
    navigate("/");
  };

  return (
    <Container className="login-wrapper">
      <div className="login-form-container">
        <Button className="back-to-home" onClick={handleBackToHome}>
          Back to Home
        </Button>
        <h2 className="login-title">Sign In</h2>
        {error && <Alert variant="danger">{error}</Alert>}
        <Form onSubmit={handleSubmit} className="login-form">
          <Form.Group controlId="formBasicEmail">
            <Form.Label>Email address</Form.Label>
            <Form.Control
              type="email"
              placeholder="Enter email"
              value={Username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={Password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </Form.Group>

          <Button variant="primary" type="submit" className="login-button">
            Sign In
          </Button>
        </Form>
        <ToastContainer />
      </div>
    </Container>
  );
}

export default SignIn;
