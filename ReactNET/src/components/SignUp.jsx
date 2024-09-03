import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import useNavigate hook
import { addUser } from "../services/userService";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./SignUp.css"; // Import your custom CSS

function SignUp() {
  const [formData, setFormData] = useState({
    FirstName: "",
    LastName: "",
    Email: "",
    Password: "",
    ContactNumber: "",
    Role: { name: "" },
  });

  const navigate = useNavigate(); // Initialize useNavigate hook

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === "Role") {
      setFormData({ ...formData, Role: { name: value } });
    } else {
      setFormData({ ...formData, [name]: value });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await addUser(formData);
      toast.success("User registered successfully!");
    } catch (error) {
      console.error("Error signing up:", error);
      toast.error(
        error.response?.data?.message || "There was an error signing up!"
      );
    }
  };

  const handleBackToHome = () => {
    navigate("/"); // Navigate to home page
  };

  return (
    <div className="signup-container">
      <button className="back-to-home" onClick={handleBackToHome}>
        Back to Home
      </button>
      <h2>Sign Up</h2>
      <form onSubmit={handleSubmit} className="signup-form">
        <div className="form-group-row">
          <div className="form-group">
            <label>First Name:</label>
            <input
              type="text"
              name="FirstName"
              value={formData.FirstName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Last Name:</label>
            <input
              type="text"
              name="LastName"
              value={formData.LastName}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        <div className="form-group-row">
          <div className="form-group">
            <label>Email:</label>
            <input
              type="email"
              name="Email"
              value={formData.Email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label>Password:</label>
            <input
              type="password"
              name="Password"
              value={formData.Password}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        <div className="form-group-row">
          <div className="form-group">
            <label>Contact Number:</label>
            <input
              type="text"
              name="ContactNumber"
              value={formData.ContactNumber}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        <div className="form-group-row">
          <div className="form-group">
            <label>Role:</label>
            <select
              name="Role"
              value={formData.Role.name}
              onChange={handleChange}
              required
            >
              <option value="">Select a role</option>
              <option value="ADMIN">ADMIN</option>
              <option value="USER">USER</option>
            </select>
          </div>
        </div>
        <button type="submit">Sign Up</button>
      </form>
      <ToastContainer />
    </div>
  );
}

export default SignUp;
