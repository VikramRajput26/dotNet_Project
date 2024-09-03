import { myAxios } from "./helper";

export const login = async (loginRequest) => {
  try {
    const response = await myAxios.post("/api/auth/login", loginRequest, {
      headers: {
        "Content-Type": "application/json", // Explicitly setting Content-Type
      },
    });
    return response.data;
  } catch (error) {
    throw error; // Ensures the error is properly caught in the component
  }
};

export const addUser = async (createUserDTO) => {
  try {
    const response = await myAxios.post("/api/auth/addUser", createUserDTO, {
      headers: {
        "Content-Type": "application/json", // Explicitly setting Content-Type
      },
    });
    return response.data;
  } catch (error) {
    throw error; // Ensures the error is properly caught in the component
  }
};

export const getUserById = async (id) => {
  try {
    const response = await myAxios.get(`/api/auth/getUserById/${id}`);
    return response.data;
  } catch (error) {
    console.error(
      "Error in getUserById:",
      error.response ? error.response.data : error.message
    );
    throw error; // Ensure error is properly propagated
  }
};

export const getAllUsers = async () => {
  try {
    const response = await myAxios.get("/api/auth/getAllUsers");
    return response.data;
  } catch (error) {
    throw error; // Ensures the error is properly caught in the component
  }
};

export const getAllDoctor = async () => {
  try {
    const response = await myAxios.get("/api/auth/getAllDoctors");
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateUser = async (id, updateUserDTO) => {
  try {
    const response = await myAxios.put(
      `/api/auth/updateUser/${id}`,
      updateUserDTO,
      {
        headers: {
          "Content-Type": "application/json", // Explicitly setting Content-Type
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error updating user:", error.response || error.message);
    throw error;
  }
};

export const deleteUser = async (id) => {
  try {
    const response = await myAxios.delete(`/api/auth/deleteUser/${id}`);
    return response.data;
  } catch (error) {
    throw error; // Ensures the error is properly caught in the component
  }
};
