import React, { useEffect, useState } from "react";
import { Table, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { deleteAppointment, getAllAppointments } from "../services/aptService"; // Update path as needed
import "./AppointmentList.css"; // Create this CSS file if needed for styling

const AppointmentList = () => {
  const [appointments, setAppointments] = useState([]);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const data = await getAllAppointments();
        setAppointments(data);
      } catch (error) {
        setError("Error fetching appointments.");
        console.error("Error fetching appointments:", error);
      }
    };

    fetchAppointments();
  }, []);

  const handleViewClick = (appointmentId) => {
    navigate(`/view-appointment/${appointmentId}`);
  };

  const handleUpdateClick = (appointmentId) => {
    navigate(`/edit-appointment/${appointmentId}`);
  };
  const handleDeleteClick = async (appointmentId) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        await deleteAppointment(userId); // Call the delete service
        setAppointments(
          appointments.filter(
            (appointment) => appointment.appointmentId !== appointmentId
          )
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
    <div className="appointment-list-container">
      <h2>Appointment List</h2>
      {error && <p className="error">{error}</p>}
      <Table striped bordered hover size="sm">
        <thead>
          <tr>
            <th>#</th>
            <th>Reason</th>
            <th>Status</th>
            <th>Child ID</th>
            <th>Doctor ID</th>
            <th>Vaccine ID</th>
            <th>Appointment Date</th>
            <th>Appointment Time</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {appointments.map((appointment, index) => (
            <tr key={appointment.appointmentId}>
              <td>{index + 1}</td>
              <td>{appointment.reason}</td>
              <td>{appointment.status}</td>
              <td>{appointment.childId}</td>
              <td>{appointment.doctorId}</td>
              <td>{appointment.vaccineId}</td>
              <td>
                {new Date(appointment.appointmentDate).toLocaleDateString()}
              </td>
              <td>{appointment.appointmentTime}</td>
              <td>
                <Button
                  variant="info"
                  onClick={() => handleViewClick(appointment.appointmentId)}
                >
                  View
                </Button>
                <Button
                  variant="warning"
                  onClick={() => handleUpdateClick(appointment.appointmentId)}
                >
                  Update
                </Button>
                <Button
                  variant="danger"
                  className="action-button"
                  onClick={() => handleDeleteClick(appointment.appointmentId)}
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

export default AppointmentList;
