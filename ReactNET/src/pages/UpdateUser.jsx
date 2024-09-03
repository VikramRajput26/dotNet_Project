import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getUserById, updateUser } from "../services/userService";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import "./UpdateUser.css";

const UpdateUser = () => {
  const { userId } = useParams(); // Extract userId from URL params
  const navigate = useNavigate();
  const [user, setUser] = useState({
    userId: "",
    firstName: "",
    lastName: "",
    contactNumber: "",
    email: "",
    password: "",
    role: "", // Updated to handle role as a single string
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      if (!userId) {
        setError("User ID is missing");
        setLoading(false);
        return;
      }
      try {
        const userData = await getUserById(userId);
        setUser(userData);
        setLoading(false);
      } catch (error) {
        setError("Failed to load user data");
        setLoading(false);
      }
    };

    fetchUser();
  }, [userId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser({ ...user, [name]: value });
  };

  const handleBackClick = () => {
    navigate("/user-list"); // Navigate back to UserList page
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Prepare the DTO for submission
    const userDTO = {
      UserId: user.userId, // Ensure this matches the property name expected by the server
      FirstName: user.firstName,
      LastName: user.lastName,
      Email: user.email,
      Password: user.password,
      ContactNumber: user.contactNumber,
      Role: {
        Name: user.role, // Send role as an object with 'Name' property
      },
    };

    try {
      await updateUser(userId, userDTO);
      navigate("/user-list");
    } catch (error) {
      setError("Failed to update user");
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="update-user-container">
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="formUserId">
          <Form.Label>User ID</Form.Label>
          <Form.Control
            type="text"
            name="userId"
            value={user.userId}
            onChange={handleChange}
            readOnly
          />
        </Form.Group>
        <Form.Group controlId="formFirstName">
          <Form.Label>First Name</Form.Label>
          <Form.Control
            type="text"
            name="firstName"
            value={user.firstName}
            onChange={handleChange}
            required
          />
        </Form.Group>
        <Form.Group controlId="formLastName">
          <Form.Label>Last Name</Form.Label>
          <Form.Control
            type="text"
            name="lastName"
            value={user.lastName}
            onChange={handleChange}
            required
          />
        </Form.Group>
        <Form.Group controlId="formContactNumber">
          <Form.Label>Contact Number</Form.Label>
          <Form.Control
            type="text"
            name="contactNumber"
            value={user.contactNumber}
            onChange={handleChange}
            required
          />
        </Form.Group>
        <Form.Group controlId="formEmail">
          <Form.Label>Email</Form.Label>
          <Form.Control
            type="email"
            name="email"
            value={user.email}
            onChange={handleChange}
            required
          />
        </Form.Group>
        <Form.Group controlId="formPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            name="password"
            value={user.password}
            onChange={handleChange}
          />
        </Form.Group>
        <Form.Group controlId="formRole">
          <Form.Label>Role</Form.Label>
          <Form.Control
            as="select"
            name="role"
            value={user.role}
            onChange={handleChange}
            required
          >
            <option value="">Select Role</option>
            <option value="ADMIN">ADMIN</option>
            <option value="USER">USER</option>
          </Form.Control>
        </Form.Group>
        <Button variant="primary" type="submit">
          Update
        </Button>
        <div className="button-group">
          <Button variant="success" onClick={handleBackClick}>
            Back to User List
          </Button>
        </div>
      </Form>
    </div>
  );
};

export default UpdateUser;
