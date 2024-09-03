import { myAxios } from "./helper";

// Get all children
export const getAllChildren = async () => {
  try {
    const response = await myAxios.get("/api/child/getallchild");
    return response.data;
  } catch (error) {
    console.error("Error fetching all children:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Get a child by ID
export const getChildById = async (id) => {
  try {
    const response = await myAxios.get(`/api/child/getbyid/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching child by ID:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Add a new child
export const addChild = async (createChildDTO) => {
  try {
    const response = await myAxios.post("/api/child/addchild", createChildDTO, {
      headers: {
        "Content-Type": "application/json", // Explicitly setting Content-Type
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error adding child:", error);
    throw error;
  }
};

// Update an existing child
export const updateChild = async (id, childDTO) => {
  try {
    const response = await myAxios.put(`/api/child/update/${id}`, childDTO, {
      headers: {
        "Content-Type": "application/json", // Explicitly setting Content-Type
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error updating child:", error);
    throw error; // Ensure error is properly propagated
  }
};

// Delete a child
export const deleteChild = async (id) => {
  try {
    const response = await myAxios.delete(`/api/child/delete/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error deleting child:", error);
    throw error; // Ensure error is properly propagated
  }
};
