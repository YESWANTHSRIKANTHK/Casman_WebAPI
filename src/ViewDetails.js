import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams, Link } from 'react-router-dom';

export default function ViewDetails() {
  const { studentid } = useParams();
  const [student, setStudent] = useState(null);

  useEffect(() => {
    axios.get(`http://localhost:5038/api/Students/${studentid}`)
      .then(res => setStudent(res.data))
      .catch(err => console.error(err));
  }, [studentid]);

  if (!student) return <p>Loading...</p>;

  return (
    <div className="container mt-5">
      <h2>Student Details</h2>
      <ul className="list-group">
        <li className="list-group-item"><strong>ID:</strong> {student.id}</li>
        <li className="list-group-item"><strong>Name:</strong> {student.name}</li>
        <li className="list-group-item"><strong>Place:</strong> {student.place}</li>
        <li className="list-group-item"><strong>Phone:</strong> {student.phone}</li>
      </ul>
      <Link to="/" className="btn btn-secondary mt-3">Back</Link>
    </div>
  );
}
