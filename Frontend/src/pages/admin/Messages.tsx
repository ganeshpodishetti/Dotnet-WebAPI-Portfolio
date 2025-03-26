import React, { useEffect, useState } from 'react';
import { messageService } from '../../services/api';

const AdminMessages = () => {
  const [messages, setMessages] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedMessage, setSelectedMessage] = useState<any | null>(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchMessages();
  }, []);

  const fetchMessages = async () => {
    try {
      const data = await messageService.getAll();
      setMessages(data);
    } catch (error) {
      console.error('Error fetching messages:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this message?')) {
      try {
        await messageService.delete(id);
        fetchMessages();
      } catch (error) {
        console.error('Error deleting message:', error);
      }
    }
  };

  const handleMarkAsRead = async (id: string, isRead: boolean) => {
    try {
      await messageService.markAsRead(id, { isRead });
      fetchMessages();
    } catch (error) {
      console.error('Error updating message:', error);
    }
  };

  const viewMessage = (message: any) => {
    setSelectedMessage(message);
    setShowModal(true);
    
    // If message is unread, mark it as read when viewing
    if (!message.isRead) {
      handleMarkAsRead(message.id, true);
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h1 className="mb-4">Messages</h1>

      {messages.length === 0 ? (
        <div className="alert alert-info">No messages found.</div>
      ) : (
        <div className="table-responsive">
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Status</th>
                <th>Name</th>
                <th>Email</th>
                <th>Subject</th>
                <th>Message</th>
                <th>Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {messages.map((message) => (
                <tr key={message.id} className={!message.isRead ? 'table-primary' : ''}>
                  <td>
                    {!message.isRead ? (
                      <span className="badge bg-primary">New</span>
                    ) : (
                      <span className="badge bg-secondary">Read</span>
                    )}
                  </td>
                  <td>{message.name}</td>
                  <td>
                    <a href={`mailto:${message.email}`}>{message.email}</a>
                  </td>
                  <td>{message.subject || '-'}</td>
                  <td>
                    {message.content.length > 50
                      ? `${message.content.substring(0, 50)}...`
                      : message.content}
                  </td>
                  <td>{new Date(message.createdAt).toLocaleDateString()}</td>
                  <td>
                    <button
                      className="btn btn-sm btn-outline-primary me-2"
                      onClick={() => viewMessage(message)}
                    >
                      View
                    </button>
                    {message.isRead ? (
                      <button
                        className="btn btn-sm btn-outline-secondary me-2"
                        onClick={() => handleMarkAsRead(message.id, false)}
                      >
                        Mark as Unread
                      </button>
                    ) : (
                      <button
                        className="btn btn-sm btn-outline-success me-2"
                        onClick={() => handleMarkAsRead(message.id, true)}
                      >
                        Mark as Read
                      </button>
                    )}
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(message.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Message Detail Modal */}
      {showModal && selectedMessage && (
        <div className="modal show" style={{ display: 'block' }} tabIndex={-1}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {selectedMessage.subject || 'No Subject'}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setShowModal(false)}
                ></button>
              </div>
              <div className="modal-body">
                <p><strong>From:</strong> {selectedMessage.name} ({selectedMessage.email})</p>
                <p><strong>Date:</strong> {new Date(selectedMessage.createdAt).toLocaleString()}</p>
                <hr />
                <p>{selectedMessage.content}</p>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={() => window.open(`mailto:${selectedMessage.email}?subject=Re: ${selectedMessage.subject || 'Your message'}`)}
                >
                  Reply by Email
                </button>
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={() => setShowModal(false)}
                >
                  Close
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
      
      {/* Modal backdrop */}
      {showModal && <div className="modal-backdrop fade show"></div>}
    </div>
  );
};

export default AdminMessages;
