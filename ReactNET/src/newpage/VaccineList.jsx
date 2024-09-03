import React, { useEffect, useState } from "react";
import { Table, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { deleteVaccine, getAllVaccines } from "../services/vaccineService"; // Update path as needed
import "./VaccineList.css"; // Create this CSS file if needed for styling

const VaccineList = () => {
  const [vaccines, setVaccines] = useState([]);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchVaccines = async () => {
      try {
        const data = await getAllVaccines();
        setVaccines(data);
      } catch (error) {
        setError("Error fetching vaccines.");
        console.error("Error fetching vaccines:", error);
      }
    };

    fetchVaccines();
  }, []);

  const handleViewClick = (vaccineId) => {
    navigate(`/view-vaccine/${vaccineId}`);
  };

  const handleUpdateClick = (vaccineId) => {
    navigate(`/edit-vaccine/${vaccineId}`);
  };

  const handleDeleteClick = async (vaccineId) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        await deleteVaccine(vaccineId); // Call the delete service
        setVaccines(
          vaccines.filter((vaccine) => vaccine.vaccineId !== vaccineId)
        ); // Update local state
      } catch (error) {
        setError("Error deleting user.");
        console.error("Error deleting user:", error);
      }
    }
  };

  const handleBackClick = () => {
    navigate("/");
  };

  return (
    <div className="vaccine-list-container">
      <h2>Vaccine List</h2>
      {error && <p className="error">{error}</p>}
      <Table striped bordered hover size="sm">
        <thead>
          <tr>
            <th>#</th>
            <th>Vaccine Name</th>
            <th>Description</th>
            <th>Recommended Age</th>
            <th>Age Unit</th>
            <th>Side Effects</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {vaccines.map((vaccine, index) => (
            <tr key={vaccine.vaccineId}>
              <td>{index + 1}</td>
              <td>{vaccine.vaccineName}</td>
              <td>{vaccine.description}</td>
              <td>{vaccine.recommendedAge}</td>
              <td>{vaccine.ageUnit}</td>
              <td>{vaccine.sideEffects}</td>
              <td>
                <Button
                  variant="info"
                  onClick={() => handleViewClick(vaccine.vaccineId)}
                >
                  View
                </Button>
                <Button
                  variant="warning"
                  onClick={() => handleUpdateClick(vaccine.vaccineId)}
                >
                  Update
                </Button>
                <Button
                  variant="danger"
                  className="action-button"
                  onClick={() => handleDeleteClick(vaccine.vaccineId)}
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

export default VaccineList;
