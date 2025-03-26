import React, { useState } from 'react';
import { messageService } from '../services/api';
import { MessageRequestDto } from '../types';

const Contact = () => {
  const [message, setMessage] = useState<MessageRequestDto>({
    name: '',
    email: '',
    subject: '',
    content: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitStatus, setSubmitStatus] = useState<'success' | 'error' | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await messageService.create(message);
      setSubmitStatus('success');
      setMessage({ name: '', email: '', subject: '', content: '' });
    } catch (error) {
      setSubmitStatus('error');
      console.error('Failed to send message:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="container">
      <h2>Contact Me</h2>
      {submitStatus === 'success' && (
        <div className="alert alert-success">Message sent successfully!</div>
      )}
      {submitStatus === 'error' && (
        <div className="alert alert-danger">Failed to send message. Please try again.</div>
      )}
      <div className="row">
        <div className="col-md-6">
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="name" className="form-label">Name</label>
              <input
                type="text"
                className="form-control"
                id="name"
                value={message.name}
                onChange={(e) => setMessage({ ...message, name: e.target.value })}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="email" className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                id="email"
                value={message.email}
                onChange={(e) => setMessage({ ...message, email: e.target.value })}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="subject" className="form-label">Subject (optional)</label>
              <input
                type="text"
                className="form-control"
                id="subject"
                value={message.subject}
                onChange={(e) => setMessage({ ...message, subject: e.target.value })}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="content" className="form-label">Message</label>
              <textarea
                className="form-control"
                id="content"
                rows={5}
                value={message.content}
                onChange={(e) => setMessage({ ...message, content: e.target.value })}
                required
              ></textarea>
            </div>
            <button
              type="submit"
              className="btn btn-primary"
              disabled={isSubmitting}
            >
              {isSubmitting ? 'Sending...' : 'Send Message'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Contact;
