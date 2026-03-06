In a registerform.jsx or similar file, add:

import { useState } from 'react';

export default function RegisterForm() {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    usResidencyConfirmed: false,
  });
  const [error, setError]     = useState('');
  const [success, setSuccess] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    // Frontend guard to reinforce that which is implemented in the backend
    if (!formData.usResidencyConfirmed) {
      setError('You must confirm US residency to create an account.');
      return;
    }

    setLoading(true);

    try {
      const res = await fetch('/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
        credentials: 'include',
      });

      if (res.status === 404) {
        setError('This service is only available to US residents.');
        return;
      }

      if (!res.ok) {
        const data = await res.json();
        // Typical validation errors (e.g. password too weak)
        const messages = Object.values(data.errors ?? {})
          .flat()
          .join(' ');
        setError(messages || data.message || 'Registration failed.');
        return;
      }

      setSuccess(true);
      // Redirect to login or dashboard here

    } catch {
      setError('Network error. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return <p>Registration successful! Please check your email to confirm your account.</p>;
  }

  return (
    <form onSubmit={handleSubmit} noValidate>

      <div>
        <label htmlFor="email">Email</label>
        <input
          id="email"
          name="email"
          type="email"
          required
          autoComplete="email"
          value={formData.email}
          onChange={handleChange}
        />
      </div>

      <div>
        <label htmlFor="password">Password</label>
        <input
          id="password"
          name="password"
          type="password"
          required
          autoComplete="new-password"
          minLength={12}
          value={formData.password}
          onChange={handleChange}
        />
      </div>

      <div>
        <label>
          <input
            name="usResidencyConfirmed"
            type="checkbox"
            required
            checked={formData.usResidencyConfirmed}
            onChange={handleChange}
          />
          {' '}
          I confirm that I am a resident of the{' '}
          <strong>United States</strong>. This service is only available
          to US residents in accordance with our{' '}
          // will need to create terms to add here
		  <a href="/terms" target="_blank" rel="noopener noreferrer">
            Terms of Service
          </a>.
        </label>
      </div>

      {error && (
        <p role="alert" style={{ color: 'red' }}>{error}</p>
      )}

      <button
        type="submit"
        disabled={!formData.usResidencyConfirmed || loading}
      >
        {loading ? 'Creating account...' : 'Create Account'}
      </button>

    </form>
  );
}