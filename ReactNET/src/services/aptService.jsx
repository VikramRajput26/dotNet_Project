import { myAxios } from "./helper";

// Get all appointments
export const getAllAppointments = async () => {
  try {
    const response = await myAxios.get("/api/appointment/getallapt");
    return response.data;
  } catch (error) {
    console.error("Error fetching all appointments:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Get an appointment by ID
export const getAppointmentById = async (id) => {
  try {
    const response = await myAxios.get(`/api/appointment/getbyid/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching appointment by ID:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Add a new appointment
export const addAppointment = async (createAppointmentDTO) => {
  try {
    const response = await myAxios.post(
      "/api/appointment/addapt",
      createAppointmentDTO,
      {
        headers: {
          "Content-Type": "application/json", // Explicitly setting Content-Type
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error adding appointment:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Update an existing appointment
export const updateAppointment = async (id, appointmentDTO) => {
  try {
    const response = await myAxios.put(
      `/api/appointment/updateapt/${id}`,
      appointmentDTO,
      {
        headers: {
          "Content-Type": "application/json", // Explicitly setting Content-Type
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error updating appointment:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Delete an appointment
export const deleteAppointment = async (id) => {
  try {
    const response = await myAxios.delete(`/api/appointment/delete/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting appointment:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Removed getUserClaims as it's not defined in the AppointmentController
