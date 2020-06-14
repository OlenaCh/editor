using Gdk;
using Gtk;
using Cairo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphEditor {

    public class Vector {
        PointD a, b, unit, midpoint;
        double x, y, difference, direction;

        public Vector(PointD a,
                      PointD b,
                      double difference,
                      double direction = 1.0) {
            this.a = a;
            this.b = b;
            this.difference = difference;
            this.direction = direction;

            initValues();
        }

        public PointD arrowTipPoint() {
            return new PointD(
                a.X + difference * unit.X, a.Y + difference * unit.Y
            );
        }

        public PointD arrowCornerPoint() {
            double c = difference * 2, d = difference / 2;
            double tmpX = a.X + c * unit.X + direction * d * unit.Y,
                   tmpY = a.Y + c * unit.Y - direction * d * unit.X;
            return new PointD(tmpX, tmpY);
        }

        public PointD edgeWeightPoint() {
            double tmpX = unit.Y, tmpY = unit.X * -1;
            return new PointD(
                midpoint.X + difference * tmpX, midpoint.Y + difference * tmpY
            );
        }

        void initValues() {
            x = b.X - a.X;
            y = b.Y - a.Y;

            double s = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            unit = new PointD(x / s, y / s);
            midpoint = new PointD((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }
    }
}
