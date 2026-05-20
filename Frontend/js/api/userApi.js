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
    },

    async login(email, password) {
        const response = await fetch(`${BASE_URL}/Users/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        
        const data = await response.json();
        if (!response.ok) {
            throw new Error(data.message || 'Помилка при спробі входу.');
        }
        return data;
    },

    async register(email, password, firstName, lastName) {
        const response = await fetch(`${BASE_URL}/Users/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password, firstName, lastName })
        });
        
        const data = await response.json();
        if (!response.ok) {
            throw new Error(data.message || 'Помилка при реєстрації.');
        }
        return data;
    }
};