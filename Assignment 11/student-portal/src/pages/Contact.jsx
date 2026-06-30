import { useNavigate } from "react-router-dom";

export default function Contact() {
  const navigate = useNavigate();

  return (
    <section className="page">
      <p className="eyebrow">Support</p>
      <h1>Contact Us</h1>

      <dl className="detail-list">
        <div>
          <dt>Email</dt>
          <dd>support@studentportal.com</dd>
        </div>
        <div>
          <dt>Phone</dt>
          <dd>9876543210</dd>
        </div>
        <div>
          <dt>Location</dt>
          <dd>Chennai, India</dd>
        </div>
      </dl>

      <button className="btn btn--ghost" onClick={() => navigate(-1)}>
        Go Back
      </button>
    </section>
  );
}
