import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getAppointmentById } from "../services/aptService";
import { Button } from "react-bootstrap";
import "./ViewApt.css"; // Optional: Add your styles

const ViewApt = () => {
  const { id } = useParams();
  const [appointment, setAppointment] = useState(null);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchAppointment = async () => {
      try {
        const data = await getAppointmentById(id);
        setAppointment(data);
      } catch (error) {
        setError("Error fetching appointment.");
        console.error("Error fetching appointment:", error);
      }
    };

    fetchAppointment();
  }, [id]);

  const handleBack = () => {
    navigate("/appointments");
  };

  return (
    <div className="view-apt-container">
      <h2>Appointment Details</h2>
      {error && <p className="error">{error}</p>}
      {appointment ? (
        <div>
          <p>
            <strong>Appointment ID:</strong> {appointment.appointmentId}
          </p>
          <p>
            <strong>Reason:</strong> {appointment.reason}
          </p>
          <p>
            <strong>Status:</strong> {appointment.status}
          </p>
          <p>
            <strong>Child ID:</strong> {appointment.childId}
          </p>
          <p>
            <strong>Doctor ID:</strong> {appointment.doctorId}
          </p>
          <p>
            <strong>Vaccine ID:</strong> {appointment.vaccineId}
          </p>
          <p>
            <strong>Appointment Date:</strong>{" "}
            {new Date(appointment.appointmentDate).toLocaleDateString()}
          </p>
          <p>
            <strong>Appointment Time:</strong> {appointment.appointmentTime}
          </p>
          <Button variant="success" onClick={handleBack}>
            Back to List
          </Button>
        </div>
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
};

export default ViewApt;
