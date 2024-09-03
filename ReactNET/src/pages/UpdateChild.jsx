import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { getChildById, updateChild } from "../services/childService";
import { Button, Form } from "react-bootstrap";
import "./UpdateChild.css"; // Import your updated CSS

const UpdateChild = () => {
  const { childId } = useParams();
  const navigate = useNavigate();
  const [child, setChild] = useState({
    childId: "",
    firstName: "",
    lastName: "",
    dateOfBirth: "",
    gender: "",
    bloodType: "",
    userId: "", // Added User ID field
  });
  const [error, setError] = useState(null);

  useEffect(() => {
    if (childId) {
      fetchChildById();
    }
  }, [childId]);

  const fetchChildById = async () => {
    try {
      const data = await getChildById(childId);
      setChild(data);
    } catch (error) {
      console.error("Error fetching child details: ", error);
      toast.error("Error fetching child details");
    }
  };

  const handleUpdate = async (e) => {
    e.preventDefault();
    try {
      await updateChild(childId, child);
      toast.success("Child updated successfully");
      navigate("/child-list");
    } catch (error) {
      console.error("Error updating child: ", error);
      toast.error("Error updating child");
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setChild((prevChild) => ({
      ...prevChild,
      [name]: value,
    }));
  };

  return (
    <div className="update-child-container">
      <ToastContainer />
      <h2>Update Child</h2>
      {error && <p className="error">{error}</p>}
      {!child ? (
        <div>Loading...</div>
      ) : (
        <Form onSubmit={handleUpdate}>
          <Form.Group controlId="childId">
            <Form.Label>Child ID</Form.Label>
            <Form.Control
              type="text"
              name="childId"
              value={child.childId}
              readOnly
            />
          </Form.Group>
          <Form.Group controlId="firstName">
            <Form.Label>First Name</Form.Label>
            <Form.Control
              type="text"
              name="firstName"
              value={child.firstName}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Form.Group controlId="lastName">
            <Form.Label>Last Name</Form.Label>
            <Form.Control
              type="text"
              name="lastName"
              value={child.lastName}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Form.Group controlId="dateOfBirth">
            <Form.Label>Date of Birth</Form.Label>
            <Form.Control
              type="date"
              name="dateOfBirth"
              value={child.dateOfBirth}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Form.Group controlId="gender">
            <Form.Label>Gender</Form.Label>
            <Form.Control
              type="text"
              name="gender"
              value={child.gender}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Form.Group controlId="bloodType">
            <Form.Label>Blood Type</Form.Label>
            <Form.Control
              type="text"
              name="bloodType"
              value={child.bloodType}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Form.Group controlId="userId">
            <Form.Label>User ID</Form.Label>
            <Form.Control
              type="number"
              name="userId"
              value={child.userId}
              onChange={handleChange}
              required
            />
          </Form.Group>
          <Button variant="success" type="submit">
            Save Changes
          </Button>
          <Button variant="secondary" onClick={() => navigate("/child-list")}>
            Cancel
          </Button>
        </Form>
      )}
    </div>
  );
};

export default UpdateChild;
