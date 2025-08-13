import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';

export default function EditStudent() {
  const [student, setStudent] = useState({ name: '', place: '', phone: '' });
  const { studentid } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    axios.get(`http://localhost:5038/api/Students/${studentid}`)
      .then(res => setStudent(res.data))
      .catch(err => console.error(err));
  }, [studentid]);

  const handleChange = e => {
    setStudent({ ...student, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    await axios.put(`http://localhost:5038/api/Students/${studentid}`, student);
    navigate('/');
  };

  return (
    <div className="container mt-5">
      <h2>Edit Student</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label>Name</label>
          <input name="name" className="form-control" value={student.name} onChange={handleChange} />
        </div>
        <div className="mb-3">
          <label>Place</label>
          <input name="place" className="form-control" value={student.place} onChange={handleChange} />
        </div>
        <div className="mb-3">
          <label>Phone</label>
          <input name="phone" className="form-control" value={student.phone} onChange={handleChange} />
        </div>
        <button type="submit" className="btn btn-success">Update</button>
      </form>
    </div>
  );
}
