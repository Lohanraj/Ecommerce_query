export default function Profile() {
  return (
    <div>
      <h2>Student Profile</h2>
      <dl className="detail-list">
        <div>
          <dt>Name</dt>
          <dd>Student User</dd>
        </div>
        <div>
          <dt>Email</dt>
          <dd>student@example.com</dd>
        </div>
        <div>
          <dt>Course</dt>
          <dd>React JS Fundamentals</dd>
        </div>
        <div>
          <dt>Status</dt>
          <dd>Active</dd>
        </div>
      </dl>
    </div>
  );
}
