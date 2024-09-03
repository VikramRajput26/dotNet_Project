import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { addChild } from "../services/childService"; // Ensure this is imported correctly
import "./ChildRegister.css";
import { Button } from "react-bootstrap";

const ChildRegister = () => {
  const [child, setChild] = useState({
    FirstName: "",
    LastName: "",
    DateOfBirth: "",
    Gender: "",
    BloodType: "",
    UserId: "",
  });

  const navigate = useNavigate();

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId) {
      setChild((prevChild) => ({
        ...prevChild,
        UserId: userId,
      }));
    }
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setChild((prevChild) => ({
      ...prevChild,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await addChild(child); // Ensure addChild is the correct function
      console.log("Child registered successfully:", response);

      // Store childId in localStorage
      localStorage.setItem("childId", response.childId);

      navigate("/appointmentregister"); // Navigate to Appointment component after successful registration
    } catch (error) {
      console.error("Error registering child:", error);
    }
  };

  const goToHomePage = () => {
    navigate("/");
  };

  const goToAppointmentRegister = () => {
    navigate("/appointmentregister");
  };

  return (
    <div className="child-register-container">
      <Button
        variant="primary"
        className="child-register-home-button"
        onClick={goToHomePage}
      >
        Back to Home
      </Button>
      <h2 className="child-register-title">Register Child</h2>
      <form onSubmit={handleSubmit} method="POST">
        <div className="child-register-form-group">
          <label className="child-register-label">First Name:</label>
          <input
            type="text"
            name="FirstName" // Changed to FirstName to match DTO
            value={child.FirstName}
            onChange={handleChange}
            className="child-register-input"
            required
          />
        </div>
        <div className="child-register-form-group">
          <label className="child-register-label">Last Name:</label>
          <input
            type="text"
            name="LastName" // Changed to LastName to match DTO
            value={child.LastName}
            onChange={handleChange}
            className="child-register-input"
            required
          />
        </div>
        <div className="child-register-form-group">
          <label className="child-register-label">Date of Birth:</label>
          <input
            type="date"
            name="DateOfBirth" // Changed to DateOfBirth to match DTO
            value={child.DateOfBirth}
            onChange={handleChange}
            className="child-register-input"
            required
          />
        </div>
        <div className="child-register-form-group">
          <label className="child-register-label">Gender:</label>
          <select
            name="Gender" // Changed to Gender to match DTO
            value={child.Gender}
            onChange={handleChange}
            className="child-register-input"
            required
          >
            <option value="">Select Gender</option>
            <option value="MALE">MALE</option>
            <option value="FEMALE">FEMALE</option>
            <option value="OTHER">OTHER</option>
          </select>
        </div>
        <div className="child-register-form-group">
          <label className="child-register-label">Blood Type:</label>
          <input
            type="text"
            name="BloodType" // Changed to BloodType to match DTO
            value={child.BloodType}
            onChange={handleChange}
            className="child-register-input"
            required
          />
        </div>
        <div className="child-register-form-group">
          <label className="child-register-label">User ID:</label>
          <input
            type="text"
            name="UserId" // Changed to UserId to match DTO
            value={child.UserId}
            className="child-register-input"
            readOnly // Making it read-only as it's set from localStorage
          />
        </div>
        <button type="submit" className="child-register-button">
          Register Child
        </button>
      </form>
      <button
        onClick={goToAppointmentRegister}
        className="child-register-button"
      >
        Go to Appointment Registration
      </button>
    </div>
  );
};

export default ChildRegister;
