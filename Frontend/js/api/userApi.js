const BASE_URL = 'http://localhost:5156/api';

export const userApi = {
    async getAll() {
        try {
            const response = await fetch(`${BASE_URL}/Users`);
            if (!response.ok) throw new Error('Не вдалося завантажити користувачів');
            return await response.json();
        } catch (error) {
            console.error('UserAPI Error:', error);
            throw error;
        }
    }
};