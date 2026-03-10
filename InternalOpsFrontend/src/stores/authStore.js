import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export const useAuthStore = create(
    persist(
        (set) => ({
            user: null,
            accessToken: null,

            setAuth({ token, email, role }) {
                localStorage.setItem("accessToken", token);
                set({ user: { email, role }, accessToken: token });
            },

            async logout() {
                localStorage.removeItem('accessToken');
                set({ user: null, accessToken: null });
            },
        }),
        {
            name: 'auth-storage',
            partialize: (state) => ({ user: state.user, accessToken: state.accessToken }),
        }
    )
);