import React, { useEffect, useState } from "react";
import { Table, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { deleteChild, getAllChildren } from "../services/childService";
import "./ChildList.css";

const ChildList = () => {
  const [children, setChildren] = useState([]);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchChildren = async () => {
      try {
        const data = await getAllChildren();
        setChildren(data);
      } catch (error) {
        setError("Error fetching children.");
        console.error("Error fetching children:", error);
      }
    };

    fetchChildren();
  }, []);

  const handleDeleteClick = async (childId) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        await deleteChild(childId); // Call the delete service
        setChildren(children.filter((child) => child.childId !== childId)); // Update local state
      } catch (error) {
        setError("Error deleting user.");
        console.error("Error deleting user:", error);
      }
    }
  };

  const handleViewClick = (childId) => {
    navigate(`/view-child/${childId}`);
  };

  const handleUpdateClick = (childId) => {
    navigate(`/update-child/${childId}`);
  };

  const handleBackClick = () => {
    navigate("/");
  };

  return (
    <div className="child-list-container">
      <h2>Child List</h2>
      {error && <p className="error">{error}</p>}
      <Table striped bordered hover size="sm">
        <thead>
          <tr>
            <th>#</th>
            <th>Child ID</th> {/* Added Child ID header */}
            <th>First Name</th>
            <th>Last Name</th>
            <th>Date of Birth</th>
            <th>Gender</th>
            <th>Blood Type</th>
            <th>User ID</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {children.map((child, index) => (
            <tr key={child.childId}>
              <td>{index + 1}</td>
              <td>{child.childId}</td> {/* Display Child ID */}
              <td>{child.firstName}</td>
              <td>{child.lastName}</td>
              <td>{new Date(child.dateOfBirth).toLocaleDateString()}</td>
              <td>{child.gender}</td>
              <td>{child.bloodType}</td>
              <td>{child.userId}</td>
              <td>
                <Button
                  variant="info"
                  onClick={() => handleViewClick(child.childId)}
                >
                  View
                </Button>
                <Button
                  variant="warning"
                  onClick={() => handleUpdateClick(child.childId)}
                >
                  Update
                </Button>
                <Button
                  variant="danger"
                  className="action-button"
                  onClick={() => handleDeleteClick(child.childId)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      <Button
        variant="success"
        className="back-button"
        onClick={handleBackClick}
      >
        Back to Home
      </Button>
    </div>
  );
};

export default ChildList;
