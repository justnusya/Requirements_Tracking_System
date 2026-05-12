const BASE_URL = 'http://localhost:5156/api';

export const projectApi = {
    async getAll() {
        try {
            const response = await fetch(`${BASE_URL}/Projects`);
            if (!response.ok) throw new Error('Помилка завантаження даних');
            return await response.json();
        } catch (error) {
            console.error('API Error (getAll):', error);
            throw error;
        }
    },

    async getById(id) {
        try {
            const response = await fetch(`${BASE_URL}/Projects/${id}`);
            if (!response.ok) throw new Error('Проєкт не знайдено');
            return await response.json();
        } catch (error) {
            console.error('API Error (getById):', error);
            throw error;
        }
    },

    async create(projectData) {
        try {
            const response = await fetch(`${BASE_URL}/Projects`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(projectData)
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Помилка сервера: ${errorText}`);
            }

            const contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                return await response.json();
            } else {
                return null;
            }
        } catch (error) {
            console.error('API Error (create):', error);
            throw error;
        }
    },

    async update(id, data) {
        try {
            const response = await fetch(`${BASE_URL}/Projects/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ ...data, Id: id }) 
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Помилка оновлення: ${errorText}`);
            }

            return response.status === 204 ? null : await response.json();
        } catch (error) {
            console.error('API Error (update):', error);
            throw error;
        }
    }
};