import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  addAppointment,
  updateAppointment,
  getAppointmentById,
} from "../services/aptService";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { getAllDoctor } from "../services/userService";
import "./Appointment.css";

const Appointment = () => {
  const [appointment, setAppointment] = useState({
    Reason: "",
    Status: "",
    ChildId: "",
    DoctorId: "",
    VaccineId: "",
    AppointmentDate: "",
    AppointmentTime: "",
  });
  const [doctors, setDoctors] = useState([]);
  const [isUpdating, setIsUpdating] = useState(false);
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    const storedChildId = localStorage.getItem("childId");
    if (storedChildId) {
      setAppointment((prevAppointment) => ({
        ...prevAppointment,
        ChildId: storedChildId,
      }));
    }

    if (id) {
      setIsUpdating(true);
      fetchAppointment(id);
    }

    fetchDoctors(); // Fetch doctors when component mounts
  }, [id]);

  const fetchAppointment = async (id) => {
    try {
      const data = await getAppointmentById(id);
      setAppointment({
        Reason: data.Reason,
        Status: data.Status,
        ChildId: data.ChildId,
        DoctorId: data.DoctorId,
        VaccineId: data.VaccineId,
        AppointmentDate: data.AppointmentDate,
        AppointmentTime: data.AppointmentTime,
      });
    } catch (error) {
      console.error("Error fetching appointment:", error);
    }
  };

  const fetchDoctors = async () => {
    try {
      const doctorData = await getAllDoctor(); // Fetch doctors from API
      const formattedDoctors = doctorData.map((doctor) => ({
        id: doctor.userId,
        name: `${doctor.firstName} ${doctor.lastName}`,
      }));
      setDoctors(formattedDoctors); // Set the doctors state with formatted data
    } catch (error) {
      console.error("Error fetching doctors:", error);
      toast.error("Failed to fetch doctors. Please try again.");
    }
  };

  const handleChange = (e) => {
    setAppointment({ ...appointment, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      Reason: appointment.Reason,
      Status: appointment.Status,
      ChildId: parseInt(appointment.ChildId, 10),
      DoctorId: parseInt(appointment.DoctorId, 10),
      VaccineId: parseInt(appointment.VaccineId, 10),
      AppointmentDate: appointment.AppointmentDate,
      AppointmentTime: appointment.AppointmentTime,
    };

    try {
      if (isUpdating) {
        await updateAppointment(id, payload);
        toast.success("Appointment updated successfully!");
      } else {
        await addAppointment(payload);
        toast.success("Appointment registered successfully!");
      }
      navigate("/appointments");
    } catch (error) {
      console.error("Error saving appointment:", error);
      toast.error("Failed to save appointment. Please try again.");
    }
  };

  const handleBackToHome = () => {
    navigate("/");
  };

  return (
    <div className="appointment-container">
      <button className="back-to-home" onClick={handleBackToHome}>
        Back to Home
      </button>
      <div className="appointment-form">
        <h2>{isUpdating ? "Update Appointment" : "Create Appointment"}</h2>
        <form onSubmit={handleSubmit}>
          <label htmlFor="Reason">Reason:</label>
          <input
            type="text"
            id="Reason"
            name="Reason"
            value={appointment.Reason}
            onChange={handleChange}
            required
          />

          <label htmlFor="ChildId">Child ID:</label>
          <input
            type="number"
            id="ChildId"
            name="ChildId"
            value={appointment.ChildId}
            onChange={handleChange}
            required
          />

          <label htmlFor="DoctorId">Doctor:</label>
          <select
            id="DoctorId"
            name="DoctorId"
            value={appointment.DoctorId}
            onChange={handleChange}
            required
          >
            <option value="">Select Doctor</option>
            {doctors.map((doctor) => (
              <option key={doctor.id} value={doctor.id}>
                {doctor.name}
              </option>
            ))}
          </select>

          <label htmlFor="VaccineId">Vaccine ID:</label>
          <input
            type="number"
            id="VaccineId"
            name="VaccineId"
            value={appointment.VaccineId}
            onChange={handleChange}
            required
          />

          <label htmlFor="AppointmentDate">Date:</label>
          <input
            type="date"
            id="AppointmentDate"
            name="AppointmentDate"
            value={appointment.AppointmentDate}
            onChange={handleChange}
            required
          />

          <label htmlFor="AppointmentTime">Time (hh:mm):</label>
          <input
            type="time"
            id="AppointmentTime"
            name="AppointmentTime"
            value={appointment.AppointmentTime}
            onChange={handleChange}
            required
          />

          <label htmlFor="Status">Status:</label>
          <select
            id="Status"
            name="Status"
            value={appointment.Status}
            onChange={handleChange}
            required
          >
            <option value="">Select status</option>
            <option value="PENDING">Pending</option>
            <option value="SCHEDULED">Scheduled</option>
            <option value="COMPLETED">Completed</option>
            <option value="CANCELED">Canceled</option>
          </select>

          <button type="submit">
            {isUpdating ? "Update" : "Create"} Appointment
          </button>
        </form>
      </div>
      <ToastContainer />
    </div>
  );
};

export default Appointment;
